
using System.Collections.Generic;
using Godot;
using Vain.Core;
using Vain.HubSystem;
using Vain.HubSystem.Query;
using Vain.Log;
using Vain.Singleton;
using static Vain.HubSystem.Query.Queries;
using static Vain.HubSystem.GameEvent.GameEvents.Entity;
using Vain.HubSystem.GameEvent;
using System.Threading.Tasks;

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
                async (string entity) =>
                {
                    var agent = new ListenerAgentTracked<EntityRegisteredEventTracked,EntityTrackedArgs>();
                    var tracking = Hub.Instance.EmitTracked<EntitySpawnRequestEventTracked, EntitySpawnRequestEventTrackedArgs>(new EntitySpawnRequestEventTrackedArgs{
                        EntityName = entity,
                    });
                    agent.Listen(tracking);
                   
                    var index = await agent.Result;
                    var response = Hub.Instance.QueryData<EntityIndexQuery, EntityReferenceQueryRequest, EntityIndexQueryResponse>(new EntityReferenceQueryRequest(){ Entity = index.Entity});
                    return response?.Index.ToString();
                   
                }
            ),
            new Command
            (
                "?:s ?:f ?:f",
                async (string entity, float x, float y) =>
                {
                    
                    var agent = new ListenerAgentTracked<EntityRegisteredEventTracked,EntityTrackedArgs>();
                    var tracking = Hub.Instance.EmitTracked<EntitySpawnRequestEventTracked, EntitySpawnRequestEventTrackedArgs>(new EntitySpawnRequestEventTrackedArgs{
                        EntityName = entity,
                        Position = new Vector2(x,y),
                    });
                    agent.Listen(tracking);
                   
                    var index = await agent.Result;
                    var response = Hub.Instance.QueryData<EntityIndexQuery, EntityReferenceQueryRequest, EntityIndexQueryResponse>(new EntityReferenceQueryRequest(){ Entity = index.Entity});
                    return response?.Index.ToString();
                

                }
            ),
            new Command
            (
                "?:s ?:s",
                async (string entity,string spawnPointTag) =>
                {
                    var agent = new ListenerAgentTracked<EntityRegisteredEventTracked,EntityTrackedArgs>();
                    var tracking = Hub.Instance.EmitTracked<EntitySpawnRequestEventTracked, EntitySpawnRequestEventTrackedArgs>(new EntitySpawnRequestEventTrackedArgs{
                        EntityName = entity,
                        SpawnTag = spawnPointTag,
                    });
                    agent.Listen(tracking);
                   
                    var index = await agent.Result;
                    var response = Hub.Instance.QueryData<EntityIndexQuery, EntityReferenceQueryRequest, EntityIndexQueryResponse>(new EntityReferenceQueryRequest(){ Entity = index.Entity});
                    return response?.Index.ToString();
                }
            )
        }
    };
}