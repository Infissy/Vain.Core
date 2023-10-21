using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;

using Vain.Singleton;
using Vain.Log;
namespace Vain.Core
{
	//Should handle all the resources inside a single map/level
	
	
	public partial class LevelManager : Node
	{

		//TODO: Refactor indices so it doesn't overflow
		uint _entityIndex = 0;


		List<Character> _characters = new List<Character>();
		Dictionary<uint, IEntity> _entities = new Dictionary<uint, IEntity>();




		public ReadOnlyCollection<Character> Characters => _characters.AsReadOnly();


		public ReadOnlyCollection<IEntity> Entities => _entities.Values.ToList().AsReadOnly();

		public uint Register(Character character)
		{
			
			_characters.Add(character);

			return Register(character as IEntity);

		}
		public uint Register(IEntity entity)
		{
			_entityIndex ++;

			_entities.Add(_entityIndex, entity);

			return _entityIndex;
		}



		public void Free(Character character)
		{
			_characters.Remove(character);

			Free(character as IEntity);
		}
		public void Free(IEntity entity)
		{
			if(_entityIndex < entity.RuntimeID)
				return;

			_entities.Remove(entity.RuntimeID); 

			
		}



		public void LoadLevel(string levelKey)
		{
			var levelIndex = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference?.LevelIndex;
			PackedScene level;
			try 
			{
				level = levelIndex.IndexedEntities[levelKey] as PackedScene;
			}catch(KeyNotFoundException)
			{
				Logger.GlobalLogger.Warning($"Level with key {levelKey} not found. Couldn't load the level specified.");
				return;
			}


			foreach(var child in base.GetChildren())
			{
				base.RemoveChild(child);
				child.QueueFree();
			}

			var instance = level.Instantiate();

			this.AddChild(instance);

		}



		public override void _EnterTree()
		{
			base._EnterTree();
			SingletonManager.Register(SingletonManager.Singletons.LEVEL_MANAGER,this);
		}


	}
}
