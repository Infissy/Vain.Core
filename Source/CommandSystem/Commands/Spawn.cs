
using System.Collections.Generic;
using Godot;
using Vain.Core;
using Vain.Log;
using Vain.Singleton;


namespace Vain.CLI;
public static partial class DefaultPrograms
{
    public static readonly Program Spawn = new()
    {
        Name = "spawn",

        Description = "entity : text",

        Commands =
        {
            new Command
            (
                "?:s",
                (string entity) => 
                {
                    var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities;

                    if(!entities.ContainsKey(entity))
                    {
                        RuntimeInternalLogger.Instance.Information($"No entity found with identifier {entity}");
                        return "";
                    }

                    var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities[entity];

                    var instance = (entityPrefab as PackedScene).Instantiate();

                    SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.GetChild(0)?.AddChild(instance);

                    return (instance as IEntity).RuntimeID.ToString();
                }
            ),
            new Command
            (
                "?:s ?:f ?:f ?:f",
                (string entity,float x, float y) => 
                {
                    var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities;

                    if(!entities.ContainsKey(entity))
                    {
                        RuntimeInternalLogger.Instance.Information($"No entity found with identifier {entity}");
                        return "";
                    }

                    var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities[entity];

                    var instance = (entityPrefab as PackedScene).Instantiate();

                    SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);

                    (instance as Node2D).GlobalPosition = new Vector2(x,y);

                    return (instance as IEntity).RuntimeID.ToString();
                }
            ),
            new Command
            (
                "?:s ?:s",
                (string entity,string spawnPointTag) =>
                {
                    var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities;

                    if(!entities.ContainsKey(entity))
                    {
                        RuntimeInternalLogger.Instance.Information($"No entity found with identifier {entity}");
                        return "";
                    }

                    var spawnPoint = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.SpawnPoints.GetValueOrDefault(spawnPointTag);

                    if(spawnPoint == null)
                    {
                        RuntimeInternalLogger.Instance.Information($"No spawn point found with tag {spawnPointTag}");
                        return "";
                    }

                    var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities[entity];

                    var instance = (entityPrefab as PackedScene).Instantiate();

                    SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);

                    (instance as Node2D).GlobalPosition = spawnPoint.GlobalPosition;

                    return (instance as IEntity).RuntimeID.ToString();
                }
            )
        }
    };
}