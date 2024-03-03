using System.Linq;
using Godot;
using Vain.Core;
using Vain.HubSystem;
using Vain.Log;
using Vain.Singleton;
using static Vain.HubSystem.Query.Queries;

namespace Vain.CLI;



public static partial class DefaultPrograms
{
    //Position is a character attribute, maybe merge all attributes in a single command instead of having a separate command for each attribute?
    public static readonly Program CharacterPosition = new()
    {
        Name  = "characterpos",

        Commands =
        {
            new Command
            (
                "?:n",
                (int index) => {
                var response = Hub.Instance.QueryData<EntityQuery,EntityIndexQueryRequest,EntityReferenceQueryResponse>(new EntityIndexQueryRequest{ Index = (uint) index  });
                if(response?.Entity == null)
                {
                    RuntimeInternalLogger.Instance.Information($"No entity found with given ID ({index})");
                    return;
                }
                RuntimeInternalLogger.Instance.Information((response?.Entity as Node2D).Position.ToString());
            }),

            new Command
            (
                "?:s",
                (string code) => {

                    switch (code)
                    {
                        case "player":
                            var response = Hub.Instance.QueryData<PlayerPositionQuery, EmptyQueryRequest,PositionQueryResponse>();
                            RuntimeInternalLogger.Instance.Information( response?.Position.ToString()  ??  "Player entity not present in scene.");
                            break;

                        default:
                            RuntimeInternalLogger.Instance.Information("Entity code not found.");
                            break;
                    }
                }
            )
        }
    };
}