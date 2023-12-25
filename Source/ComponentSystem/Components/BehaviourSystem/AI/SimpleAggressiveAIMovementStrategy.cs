using Godot;

namespace Vain.Core.ComponentSystem.Behaviour;

enum NPCAIBehaviour
{
	FollowPlayer,
	KeepDistance
}

public partial class SimpleAggressiveAIMovementStrategy : AIMovementStrategy
{
	[Export]
	public float DistanceToPlayer {get; set;}
	[Export]
	NPCAIBehaviour AIBehaviourType {get;set;}

	public override Vector2 BehaviourTick(Character character,AIController controller)
	{
		if(controller.AggressionLevel < controller.HostileCharacter.Position.DistanceTo(character.GlobalPosition) )
			return character.GlobalPosition;

        //TODO: Add Jitter or random movements 
        return AIBehaviourType switch
        {
            NPCAIBehaviour.FollowPlayer => controller.HostileCharacter.GlobalPosition,
            NPCAIBehaviour.KeepDistance => controller.HostileCharacter.GlobalPosition - ((controller.HostileCharacter.GlobalPosition - character.GlobalPosition).Normalized() * DistanceToPlayer),
            _ => character.GlobalPosition,
        };
    }
}
