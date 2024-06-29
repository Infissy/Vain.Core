using Godot;
using Vain.Configuration;

namespace Vain.Core;

public partial class GameRegistry : Node
{
    public static GameRegistry Instance {get;set;}
    public IndexResource EntityIndex {get;set;}
    public IndexResource ComponentIndex {get;set;}
    public IndexResource BehaviourIndex {get;set;}
    public IndexResource LevelIndex {get;set;}


    public override void _EnterTree()
    {
        base._EnterTree();

        EntityIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.EntityIndex));
        ComponentIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.ComponentIndex));
        BehaviourIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.BehaviourIndex));
        LevelIndex = ResourceLoader.Load<IndexResource>(ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.LevelIndex));
        Instance = this;
    }
}
