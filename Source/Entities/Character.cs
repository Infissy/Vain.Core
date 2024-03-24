using System;
using System.Linq;
using System.Collections.Generic;

using Godot;

using Vain.Core.ComponentSystem;
using Vain.Singleton;
using Vain.HubSystem;

using static Vain.HubSystem.GameEvent.GameEvents.Entity;

namespace Vain.Core;


/// <summary>
/// Character is the base class for all Characters in the game.
/// </summary>
public partial class Character : CharacterBody2D , IEntity
{

    List<Component> _components;

    CharacterBehaviourComponent _behaviour;

    [Export]
    public CharacterBehaviourComponent CharacterBehaviour
    {
        get => _behaviour;
        set
        {
            _behaviour = value;
        }
    }

    public bool ActionLock {get; private set;}

    public uint RuntimeID {get; set;}

    [Signal]
    public delegate void CharacterKilledEventHandler();
    [Signal]
    public delegate void CharacterBehaviourUpdateEventHandler(CharacterBehaviourComponent OldBehaviour);

    public override void _Ready()
    {
        base._Ready();

        if(_components == null)
            LoadComponents();


        Hub.Instance.Emit<EntityInstantiatedEvent, EntityInstantiatedEventArgs>(new EntityInstantiatedEventArgs{Entity = this});


    }


    public void AddComponent(Component component)
    {
        //TODO: workaround since we are calling this function from the command runner (which is not in the main thread)
        CallDeferred(MethodName.AddChild,component);

        _components.Add(component);
    }


    public T GetComponent<T>() where T : Component
    {

        //In case we need a preload, although components in this case won't be ready, so might need some sort of warning
        if(_components == null)
            LoadComponents();

        return _components.Find((c) => c is T) as T;

    }

    public Component GetComponent(Type type)
    {

        //In case we need a preload, although components in this case won't be ready, so might need some sort of warning
        if(_components == null)
            LoadComponents();

        return _components.Find((c) => c.GetType() == type);

    }

    public IReadOnlyCollection<Component> GetComponents()
    {
        return _components.AsReadOnly();
    }

    public virtual void Kill()
    {

        EmitSignal(SignalName.CharacterKilled);

        Hub.Instance.Emit<EntityInstantiatedEvent, EntityInstantiatedEventArgs>(new EntityInstantiatedEventArgs{Entity = this});


        this.QueueFree();

    }



    void LoadComponents()
    {

        _components = new List<Component>();


        foreach ( var node in GetChildren())
        {
            if(node is Component component)
                _components.Add(component);
        }


    }

}
