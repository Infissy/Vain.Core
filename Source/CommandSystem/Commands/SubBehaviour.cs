
using System.Linq;
using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.Log;
using Vain.Singleton;

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
                    var behaviours = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.BehaviourIndex.IndexedEntities.Keys;
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
                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == id).First();

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
                    var componentSuccessfulyFetched = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.BehaviourIndex.IndexedEntities.TryGetValue(behaviourName,out GodotObject behaviourScene);
                    if(!componentSuccessfulyFetched)
                    {
                        RuntimeInternalLogger.Instance.Warning($"Behaviour with name {behaviourName} was not found.");
                        return;
                    }

                    var behaviour = (behaviourScene as IndexedResourceWrapper).Instantiate() as SubBehaviour;


                    behaviour.Name  = behaviourName;

                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.First(e => e.RuntimeID == id);
                    entity.GetComponent<CharacterBehaviourComponent>().AddChild(behaviour);

                    RuntimeInternalLogger.Instance.Information($"Component {behaviourName} successfully added.");
                }
            )
        }
    };
}