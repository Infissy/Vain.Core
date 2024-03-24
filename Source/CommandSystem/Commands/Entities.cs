
using Godot;
using Vain.Core;
using Vain.HubSystem;
using Vain.HubSystem.Query;
using Vain.Log;
using Vain.Singleton;
using static Vain.HubSystem.Query.Queries;

namespace Vain.CLI;

public static partial class DefaultPrograms
{
     public static readonly Program Entities = new()
    {
        Name  = "entities",
        Commands =
        {
            new Command
            (
                "list",
                () => {

                    var msg = "\n";

                    var response = Hub.Instance.QueryData<EntitiesInSceneQuery, EntitiesInSceneQueryRequest, EntityCollectionResponse>();

                    if(response == null)
                    {
                        RuntimeInternalLogger.Instance.Warning("No entity container present in the scene.");
                        return;
                    }
                    if(response?.Entities == null || response?.Entities.Count == 0)
                    {
                        RuntimeInternalLogger.Instance.Information("No entities present in the scene.");
                        return;
                    }

                    foreach (var entity in response?.Entities)
                    {
                        msg += $"   {entity.RuntimeID}  | {(entity as Node).Name}  \n";
                    }

                    RuntimeInternalLogger.Instance.Information(msg);



                }
            )
        }

    };

}