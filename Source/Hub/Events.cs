
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



    public static class Spell {

        public struct SpellCastEvent : IGameEvent<SpellCastEventArgs> { }
        public struct SpellCastEventArgs : IGameEventArgs
        {
            public Character Caster;
            public Vector2 Target;
            public string SpellName;
            public string Template;
        }

        public struct SpellInstantiatedEvent : IGameEvent<SpellInstantiatedEventArgs> { }
        public struct SpellInstantiatedEventArgs : IGameEventArgs
        {
            public SpellSystem.Spell Spell;
        }

        
        public struct SpellCharacterHitEvent : IGameEvent<SpellCharacterHitEventArgs> { }
        public struct SpellCharacterHitEventArgs : IGameEventArgs
        {
            public SpellSystem.Spell Spell;
            public Character Character;
        }
         
        public struct SpellCollisionEvent: IGameEvent<SpellCollisionEventArgs> { }
        public struct SpellCollisionEventArgs : IGameEventArgs
        {
            public SpellSystem.Spell Caller;
            public SpellSystem.Spell Collided;
        }
        

        public struct PlayerSpellInputEvent : IGameEvent<PlayerSpellInputEventArgs> { }
        public struct PlayerSpellInputEventArgs : IGameEventArgs
        {
            public SpellInput Input;
        }
        
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