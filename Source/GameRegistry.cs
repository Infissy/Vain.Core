using Godot;
using Vain.Configuration;
using Vain.Core.ComponentSystem;
using Vain.Singleton;

namespace Vain.Core;

public partial class GameRegistry : Node
{
    public IndexResource EntityIndex {get;set;}
    public IndexResource ComponentIndex {get;set;}
    public IndexResource BehaviourIndex {get;set;}
    public IndexResource LevelIndex {get;set;}
    public override void _Ready()
    {
        base._Ready();

        EntityIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.EntityIndex));
        ComponentIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.ComponentIndex));
        BehaviourIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.BehaviourIndex));
        LevelIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.LevelIndex));


        SingletonManager.Register(SingletonManager.Singletons.GAME_REGISTRY,this);
    }
}
