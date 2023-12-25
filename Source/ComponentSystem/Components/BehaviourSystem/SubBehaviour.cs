using Godot;

namespace Vain.Core.ComponentSystem;

public abstract partial class SubBehaviour : Node
{
    protected CharacterBehaviourComponent BehaviourComponent {get;private set;}
    public override void _Ready()
    {
        BehaviourComponent = GetParent<CharacterBehaviourComponent>();
    }
}
