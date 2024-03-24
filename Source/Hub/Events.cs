
using System;
using Godot;
using Vain.Core;
using Vain.SpellSystem;
using Vain.HubSystem.Query;
namespace Vain.HubSystem.GameEvent;

public partial class GameEvents
{
    public struct PlayerInteractEvent: IGameEvent<CharacterInteractionEventArgs> { }
    public struct CharacterInteractionEventArgs : IGameEventArgs {  /*TODO : Add interactor and target */ }

    public struct PlayerMoveEvent: IGameEvent<CharacterMovementEventArgs> { }
    public struct CharacterMovementEventArgs : IGameEventArgs  { /*TODO: Add movement informations */}

    public struct PlayerHealthUpdate : IGameEvent<CharacterHealthUpdateArgs> { }
    public struct CharacterHealthUpdateArgs : IGameEventArgs
    {
        public float Health;
    }


    public struct CharacterSpellCastEvent : IGameEvent<SpellCastEventArgs> { }
    public struct SpellCastEventArgs: IGameEventArgs
    {
        public SpellCaster Caster;
        public Spell Spell;

    }


    public struct SpellPickupEvent : IGameEvent<SpellPickupEventArgs> { }
    public struct SpellPickupEventArgs : IGameEventArgs
    {
        public SpellCaster Caster;
        public SpellChanneler Spell;
    }


    public static class Entity
    {
        public struct EntitySpawnRequestEventTracked : IGameEventTracked<EntitySpawnRequestEventTrackedArgs> {}
        public struct EntitySpawnRequestEvent : IGameEvent<EntitySpawnRequestEventArgs> { }


        public struct EntitySpawnRequestEventTrackedArgs : IEntitySpawnRequestEventArgs , IGameEventTrackedArgs
        {
            public string EntityName {get;set;}
            public string SpawnTag {get;set;}
            public Vector2  Position {get;set;}
            public uint EventID { get; set;}
        }

        public interface IEntitySpawnRequestEventArgs : IGameEventArgs
        {
            public string EntityName {get;set;}
            public string SpawnTag {get;set;}
            public Vector2  Position {get;set;}
        }

        public struct EntitySpawnRequestEventArgs : IEntitySpawnRequestEventArgs
        {
            public string EntityName { get; set; }
            public string SpawnTag { get; set; }
            public Vector2 Position { get; set; }
        }


    
        //Entity Instantiation
        public struct EntityInstantiatedEvent : IGameEvent<EntityInstantiatedEventArgs> { }
        public struct EntityInstantiatedEventTracked : IGameEventTracked<EntityInstantiatedEventTrackedArgs> { }
        public struct EntityInstantiatedEventTrackedArgs : IGameEventTrackedArgs, IEntityInstantiatedEventArgs
        {
            public uint EventID { get; set; }
            public IEntity Entity { get; set; }
        }
        public struct EntityInstantiatedEventArgs : IEntityInstantiatedEventArgs
        {
            public IEntity Entity {get;set;}
        }

        public interface IEntityInstantiatedEventArgs : IGameEventArgs
        {
            public IEntity Entity {get;set;}
        }


        // Entity Registration
        public struct EntityRegisteredEvent : IGameEvent<EntityRegisteredEventArgs> { }
        public struct EntityRegisteredEventTracked : IGameEventTracked<EntityRegisteredEventTrackedArgs> { }
        public struct EntityRegisteredEventTrackedArgs : IGameEventTrackedArgs, IEntityRegisteredEventArgs
        {
            public uint EventID { get; set; }
            public IEntity Entity { get; set; }
        }
        public struct EntityRegisteredEventArgs : IEntityRegisteredEventArgs
        {
            public IEntity Entity {get;set;}
        }

        public interface IEntityRegisteredEventArgs : IGameEventArgs
        {
            public IEntity Entity {get;set;}
        }

        //Entity Destruction
        public struct EntityDestroyedEvent : IGameEvent<EntityDestroyedEventArgs> { }
        public struct EntityDestroyedEventArgs : IGameEventArgs
        {
            public IEntity Entity;
        }





        public struct LevelChangedEvent : IGameEvent<LevelNameEventArgs> { }
        public struct LevelChangeRequestEvent : IGameEvent<LevelNameEventArgs> { }
        public struct LevelNameEventArgs : IGameEventArgs
        {
            public string LevelName;
        }
    }
}