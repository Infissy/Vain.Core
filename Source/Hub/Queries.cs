
using System.Collections.ObjectModel;
using Godot;
using Vain.Core;

namespace Vain.HubSystem.Query;

public partial class Queries
{
    public struct PositionQueryResponse : IQueryResponse
    {
        public Vector2 Position;
    }
    public struct EmptyQueryRequest : IRequest {}




    public struct MousePositionQuery : IQuery<EmptyQueryRequest, PositionQueryResponse> {}
    public struct PlayerPositionQuery : IQuery<EmptyQueryRequest, PositionQueryResponse> {}


    public struct EntitiesInSceneQuery : IQuery<EntitiesInSceneQueryRequest, EntityCollectionResponse> {}
    public struct EntitiesInSceneQueryRequest : IRequest
    {
         public enum EntitiesType
        {
            Generic,
            Character,
            SpawnPoints,
        }

        public EntitiesType Type;
    }


    public struct EntityCollectionResponse : IQueryResponse
    {
        public ReadOnlyCollection<IEntity> Entities;
    }



    public struct SpawnPointPositionQuery : IQuery<EmptyQueryRequest, PositionQueryResponse> {}
    public struct KeyQueryRequest : IRequest { public string Key; }

    public struct EntityReferenceQueryRequest : IRequest { public IEntity Entity; }
    public struct EntityIndexQueryRequest : IRequest { public uint Index; }

    public struct EntityIndexQueryResponse : IQueryResponse { public uint Index; }
    public struct EntityReferenceQueryResponse : IQueryResponse { public IEntity Entity; }

    public struct EntityIndexQuery : IQuery<EntityReferenceQueryRequest, EntityIndexQueryResponse> {}
    public struct EntityQuery : IQuery<EntityIndexQueryRequest, EntityReferenceQueryResponse> {}

}