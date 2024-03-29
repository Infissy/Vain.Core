using Godot;
using Vain.Core;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using static Vain.HubSystem.GameEvent.GameEvents.Entity;

namespace Vain.LevelSystem;

[GlobalClass]
public partial class SpawnPoint : Node2D , IEntity
{

    public uint RuntimeID {get;set;}

    [Export]
    public string Tag {get;set;}


    public override void _Ready()
    {
        base._Ready();
        Hub.Instance.Emit<EntityInstantiatedEvent, EntityArgs>(new EntityArgs{ Entity = this});
    }

    public override void _ExitTree()
    {
      
        base._ExitTree();
        Hub.Instance.Emit<EntityDestroyedEvent, EntityArgs>(new EntityArgs{ Entity = this});
    }
}
