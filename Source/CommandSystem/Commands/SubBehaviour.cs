
using System.Linq;
using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.HubSystem;
using Vain.Log;
using Vain.Singleton;
using static Vain.HubSystem.Query.Queries;

namespace Vain.CLI;

public static partial class DefaultPrograms
{
    public static readonly Program SubBehaviour = new()
    {
        Name  = "sub_behaviour",

        Description = "character id : number list/add/remove",

        Commands =
        {
            new Command
            (
                "list",
                () =>
                {
                    var behaviours = GameRegistry.Instance.BehaviourIndex.IndexedEntities.Keys;
                    var outputBehaviours = "\nAvailable Behaviour:\n";

                    foreach (var behaviour in behaviours)
                    {
                        outputBehaviours += $"      {behaviour}\n";
                    }
                    outputBehaviours += "\n";
                    RuntimeInternalLogger.Instance.Information(outputBehaviours);
                }
            ),
            new Command 
            (
                "list ?:n",
                (int id) => {


                    var result = Hub.Instance.QueryData<EntitiesInSceneQuery, EntitiesInSceneQueryRequest,EntityCollectionResponse>(
                        new EntitiesInSceneQueryRequest
                        {
                            Type = EntitiesInSceneQueryRequest.EntitiesType.Character
                        }
                    );

                    if(result == null)
                    {
                        RuntimeInternalLogger.Instance.Warning("No character list provider was found in scene. (LevelManager)");
                        return;
                    }
                    var entity = result?.Entities.First(e => e.RuntimeID == id);

                    var msg = "\n";
                    foreach (var component in (entity as Character).GetComponents())
                    {
                        msg += $"   {component.GetType().Name } \n";
                    }
                    RuntimeInternalLogger.Instance.Information(msg);
                }
            ),

            new Command 
            (
                "add ?:n ?:s",
                (int id, string behaviourName) => {
                    var componentSuccessfulyFetched = GameRegistry.Instance.BehaviourIndex.IndexedEntities.TryGetValue(behaviourName,out GodotObject behaviourScene);
                    if(!componentSuccessfulyFetched)
                    {
                        RuntimeInternalLogger.Instance.Warning($"Behaviour with name {behaviourName} was not found.");
                        return;
                    }

                    var behaviour = (behaviourScene as IndexedResourceWrapper).Instantiate() as SubBehaviour;


                    var result = Hub.Instance.QueryData<EntitiesInSceneQuery, EntitiesInSceneQueryRequest,EntityCollectionResponse>(
                        new EntitiesInSceneQueryRequest
                        {
                            Type = EntitiesInSceneQueryRequest.EntitiesType.Character
                        }
                    );

                    if(result == null)
                    {
                        RuntimeInternalLogger.Instance.Warning("No character list provider was found in scene. (LevelManager)");
                        return;
                    }


                    var entity = result.Value.Entities.First(e => e.RuntimeID == id) as Character;
                    entity.GetComponent<CharacterBehaviourComponent>().AddChild(behaviour);

                    RuntimeInternalLogger.Instance.Information($"Component {behaviourName} successfully added.");
                }
            )
        }
    };
}