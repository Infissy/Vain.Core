using System.Linq;
using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.Log;
using Vain.Singleton;

namespace Vain.CLI;

public static partial class DefaultPrograms
{

    public static readonly Program Components = new()
    {
        Name  = "component",

        Description = "character id : number list/add/remove",
        Commands =
        {
            new Command
            (
                "list",
                () =>
                {
                    var components = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.ComponentIndex.IndexedEntities.Keys;

                    var outputComponents = "\nAvailable Components:\n";

                    foreach (var component in components)
                    {
                        outputComponents += $"      {component}\n";
                    }
                    outputComponents += "\n";
                    RuntimeInternalLogger.Instance.Information(outputComponents);
                }
            ),
            new Command 
            (
                "list ?:n",
                (int id) => {
                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference
                                    .Characters.First(e => e.RuntimeID == id);

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
                (int id, string componentName) => {
                    var componentSuccessfulyFetched = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference
                                                        .ComponentIndex.IndexedEntities.TryGetValue(componentName,out GodotObject componentScene);
                    if(!componentSuccessfulyFetched)
                    {
                        RuntimeInternalLogger.Instance.Warning($"No component found with name '{componentName}.'");
                        return;
                    }

                    var component = (componentScene as IndexedResourceWrapper).Instantiate() as Component;
                    component.Name = componentName;

                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.First(e => e.RuntimeID == id);

                    entity.AddComponent(component);

                    RuntimeInternalLogger.Instance.Information($"Component {componentName} successfully added.");
                }
            )
        }
    };
}