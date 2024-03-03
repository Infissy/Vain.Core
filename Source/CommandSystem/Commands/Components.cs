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
                    var components = GameRegistry.Instance.ComponentIndex.IndexedEntities.Keys;

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
                    var response = Hub.Instance.QueryData<EntityQuery,EntityIndexQueryRequest,EntityReferenceQueryResponse>(new EntityIndexQueryRequest{ Index = (uint) id  });

                    var msg = "\n";
                    if(response == null)
                    {
                        RuntimeInternalLogger.Instance.Warning("No entity container in scene.");
                        return;
                    }   
                    if(response?.Entity == null || !(response?.Entity is Character))
                    {
                        RuntimeInternalLogger.Instance.Warning($"No character in scene with id : {id}.");
                        return;
                    }

                    foreach (var component in (response?.Entity as Character).GetComponents())
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
                    var componentSuccessfulyFetched = GameRegistry.Instance
                                                        .ComponentIndex.IndexedEntities.TryGetValue(componentName,out GodotObject componentScene);
                    if(!componentSuccessfulyFetched)
                    {
                        RuntimeInternalLogger.Instance.Warning($"No component found with name '{componentName}.'");
                        return;
                    }

                    var component = (componentScene as IndexedResourceWrapper).Instantiate() as Component;
                    component.Name = componentName;


                    var response = Hub.Instance.QueryData<EntityQuery,EntityIndexQueryRequest,EntityReferenceQueryResponse>(new EntityIndexQueryRequest{ Index = (uint) id  });

                    if(response == null)
                    {
                        RuntimeInternalLogger.Instance.Warning("No entity container in scene.");
                        return;
                    }
                    if(response?.Entity == null || !(response?.Entity is Character))
                    {
                        RuntimeInternalLogger.Instance.Warning($"No character in scene with id : {id}.");
                        return;
                    }


                    (response?.Entity as Character)?.AddComponent(component);

                    RuntimeInternalLogger.Instance.Information($"Component {componentName} successfully added.");
                }
            )
        }
    };
}