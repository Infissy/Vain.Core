
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
        public struct EntityInstantiatedEvent : IGameEvent<EntityArgs> { }
        public struct EntityInstantiatedEventTracked : IGameEventTracked<EntityTrackedArgs> { }
   


        // Entity Registration
        public struct EntityRegisteredEvent : IGameEvent<EntityArgs> { }
        public struct EntityRegisteredEventTracked : IGameEventTracked<EntityTrackedArgs> { }


        //Entity Destruction
        public struct EntityDestroyedEvent : IGameEvent<EntityArgs> { }

        public struct EntityTrackedArgs : IGameEventTrackedArgs
        {
            public uint EventID { get; set; }
            public IEntity Entity { get; set; }
        }

        public struct EntityArgs : IGameEventArgs
        {
            public IEntity Entity {get;set;}
        }





        public struct LevelChangedEvent : IGameEvent<LevelNameEventArgs> { }
        public struct LevelChangeRequestEvent : IGameEvent<LevelNameEventArgs> { }
        public struct LevelNameEventArgs : IGameEventArgs
        {
            public string LevelName;
        }
    }
}