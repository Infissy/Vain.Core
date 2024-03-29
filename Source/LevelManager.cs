using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;


using Vain.Log;
using Vain.LevelSystem;
using Vain.HubSystem.GameEvent;
using static Vain.HubSystem.GameEvent.GameEvents;
using Vain.HubSystem.Query;
using static Vain.HubSystem.Query.Queries;
using static Vain.HubSystem.GameEvent.GameEvents.Entity;

using Vain.HubSystem;

namespace Vain.Core;

//Should handle all the resources inside a single map/level

public partial class LevelManager : SubViewport,
	IListener<EntityInstantiatedEvent,EntityArgs>,
	IListener<EntityDestroyedEvent,EntityArgs>,
	IListener<LevelChangeRequestEvent,LevelNameEventArgs>,
	IListenerTracked<EntitySpawnRequestEventTracked, EntitySpawnRequestEventTrackedArgs>,
	IDataProvider<EntityIndexQuery,EntityReferenceQueryRequest, EntityIndexQueryResponse>,
	IDataProvider<EntityQuery, EntityIndexQueryRequest, EntityReferenceQueryResponse>,
	IDataProvider<EntitiesInSceneQuery,EntitiesInSceneQueryRequest, EntityCollectionResponse>
{

	//TODO: Refactor indices so it doesn't overflow
	uint _entityIndex = 0;


	/// Core fields 

    readonly List<Character> _characters = new();
	readonly Dictionary<string ,SpawnPoint> _spawnPoints = new();
	readonly Dictionary<uint, IEntity> _entities = new();
	readonly Dictionary<IEntity,uint> _earlyInitializedEntities = new();


	/// Event Query
	readonly Dictionary<IEntity,uint> _entityInstantitationTracking = new();
	readonly List<uint> _handles = new();


	public ReadOnlyCollection<Character> Characters => _characters.AsReadOnly();
	public ReadOnlyCollection<IEntity> Entities => _entities.Values.ToList().AsReadOnly();
	public ReadOnlyDictionary<string,SpawnPoint> SpawnPoints => new(_spawnPoints);

    public override void _EnterTree()
    {
        base._EnterTree();
		Hub.Instance.Subscribe<EntityInstantiatedEvent,EntityArgs>(this);
		Hub.Instance.Subscribe<EntityDestroyedEvent,EntityArgs>(this);
		Hub.Instance.Subscribe<EntitySpawnRequestEventTracked,EntitySpawnRequestEventTrackedArgs>(this);
		Hub.Instance.Subscribe<LevelChangeRequestEvent,LevelNameEventArgs>(this);
		Hub.Instance.RegisterDataProvider<EntityIndexQuery,EntityReferenceQueryRequest, EntityIndexQueryResponse>(this);
		Hub.Instance.RegisterDataProvider<EntityQuery, EntityIndexQueryRequest, EntityReferenceQueryResponse>(this);
		Hub.Instance.RegisterDataProvider<EntitiesInSceneQuery,EntitiesInSceneQueryRequest, EntityCollectionResponse>(this);
    }

	// Register a new entity and return its unique identifier.
	// 
	// Parameters:
	//   entity - The entity to register.
	// 
	// Returns:
	//   The unique identifier of the registered entity. If the entity has already an identifier, it will return that identifier.
	public uint Register(IEntity entity)

	{
		//Hacky way to avoid entity duplication
		if(entity.RuntimeID != 0)
			return entity.RuntimeID;

		_entityIndex ++;

		_entities.Add(_entityIndex, entity);

		entity.RuntimeID = _entityIndex;

			
		if(entity is Character character)
			_characters.Add(character);
		
		if(entity is SpawnPoint spawnPoint)
			_spawnPoints[spawnPoint.Tag] = spawnPoint;

		if(_entityInstantitationTracking.TryGetValue(entity,out var tracking))
		{
			Hub.Instance.EmitTracked<EntityRegisteredEventTracked,EntityTrackedArgs>(new EntityTrackedArgs{ Entity = entity, EventID  = tracking});
			_entityInstantitationTracking.Remove(entity);
		}
		return _entityIndex;
	}



    public override void _Process(double delta)
    {
        base._Process(delta);
    }


    public void Unregister(Character character)
	{
		_characters.Remove(character);

		Unregister(character as IEntity);
	}

	
	public void Unregister(IEntity entity)
	{
		if(_entityIndex < entity.RuntimeID)
			return;

		_entities.Remove(entity.RuntimeID);

	}



	public void LoadLevel(string levelKey)
	{
		var levelIndex = GameRegistry.Instance.LevelIndex;
		PackedScene level;
		try
		{
			level = levelIndex.IndexedEntities[levelKey] as PackedScene;
		}catch(KeyNotFoundException)
		{
			RuntimeInternalLogger.Instance.Warning($"Level with key {levelKey} not found. Couldn't load the level specified.");
			return;
		}


		foreach(var child in base.GetChildren())
		{
			base.RemoveChild(child);
			child.QueueFree();
		}

		var instance = level.Instantiate();

		instance.Ready += () => {
			Hub.Instance.Emit<LevelChangedEvent,LevelNameEventArgs>(new LevelNameEventArgs{ LevelName = levelKey });
		};
		
		this.AddChild(instance);
	

	}




	IEntity SpawnEntity(string entityKey, string spawnPoint = "" , Vector2 position = default)
	{


		var spawnPosition = position;

		if(!string.IsNullOrEmpty(spawnPoint))
		{
			if(!_spawnPoints.TryGetValue(spawnPoint, out var spawnPointInstance))
			{
				RuntimeInternalLogger.Instance.Information($"No spawn point found with identifier {spawnPoint}");
				return null;
			}
			spawnPosition = spawnPointInstance.Position;
		}

		var present = GameRegistry.Instance.EntityIndex.IndexedEntities.TryGetValue(entityKey, out var entityPrefab);

		if(!present)
		{
			RuntimeInternalLogger.Instance.Information($"No entity found with identifier {entityKey}");
			return null;
		}

		var instance = (entityPrefab as PackedScene).Instantiate();

		AddChild(instance);

		(instance as Node2D).GlobalPosition = spawnPosition;

		return instance as IEntity;
	}


    public EntityCollectionResponse? Provide(EntitiesInSceneQueryRequest request)
    {
        return new EntityCollectionResponse{
			Entities = Entities
		};
    }
	
    public EntityIndexQueryResponse? Provide(EntityReferenceQueryRequest request)
    {
        var kv = _entities.FirstOrDefault(e => e.Value == request.Entity);

		if(!kv.Equals(default(KeyValuePair<uint, IEntity>)))
			return new EntityIndexQueryResponse	{ Index = kv.Key };

		var newIndex = Register(request.Entity);
		_earlyInitializedEntities.Add(request.Entity,newIndex);
		return new EntityIndexQueryResponse	{ Index = newIndex };

	}

    public EntityReferenceQueryResponse? Provide(EntityIndexQueryRequest request)
    {
		var queryRes = _entities.TryGetValue(request.Index, out var entity);

		if(!queryRes)
			return null;


		return new EntityReferenceQueryResponse	{ Entity = entity };
    }

  	void IListener<LevelChangeRequestEvent,LevelNameEventArgs>.HandleEvent<LevelChangeRequestEvent>(LevelNameEventArgs args)
    {
		LoadLevel(args.LevelName);
    }
	void IListener<EntitySpawnRequestEventTracked,EntitySpawnRequestEventTrackedArgs>.HandleEvent<EntitySpawnRequestEventTracked>(EntitySpawnRequestEventTrackedArgs args)
    {
		SpawnEntity(args.EntityName, args.SpawnTag, args.Position);
    }

	

    void IListener<EntityInstantiatedEvent,EntityArgs>.HandleEvent<EntityInstantiatedEvent>(EntityArgs args)
    {
		args.Entity.RuntimeID = Register(args.Entity);
	}

    
	void IListener<EntityDestroyedEvent,EntityArgs>.HandleEvent<EntityDestroyedEvent>(EntityArgs args)
    {
		Unregister(args.Entity);
    }

	public void HandleEventTracked<EntitySpawnRequestEventTracked>(EntitySpawnRequestEventTrackedArgs args)
    {
		var entity = SpawnEntity(args.EntityName, args.SpawnTag, args.Position);

		_entityInstantitationTracking[entity] = args.EventID;
	}

    
}

