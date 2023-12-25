using Godot;

namespace Vain.Core.ComponentSystem.Behaviour;

public abstract partial class AIMovementStrategy : Resource
{
    public abstract Vector2 BehaviourTick(Character character,AIController controller);
}
