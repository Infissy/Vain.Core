using Godot;
using Vain.Core;
using Vain.HubSystem;
using static Vain.HubSystem.GameEvent.GameEvents.Entity;

namespace Vain.SpellSystem;


public enum SpellInput {
    Top,
    Down,
    Left,
    Right
}


public enum Element {
    Ice,
    Fire,
    Air,
    Earth,
}



public partial class Spell : Area2D, IEntity
{
   
    public uint RuntimeID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    public Element Element { get; set; }


    private Spell() {}


    public override void _Ready(){
        base._Ready();
        
        Hub.Instance.Emit<EntityInstantiatedEvent, EntityArgs>(new EntityArgs{Entity = this});
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        Hub.Instance.Emit<EntityInstantiatedEvent, EntityArgs>(new EntityArgs{Entity = this});
    }
}


public class SpellFactory {
    
}