using Godot;
using System.Linq;
using Vain.Console;

namespace Vain.Core.ComponentSystem.Behaviour;


public partial class AIController : SubBehaviour
{
	[Export]
	internal float AggressionLevel {get;set;}
	[Export]
	AIMovementStrategy AI {get;set;}

	[Export]
	internal float AvoidanceDistance{get; set;}

	[Export]
	Area2D _avoidanceArea;

	internal Character HostileCharacter{get;set;}
	public override void _Ready()
	{
		_avoidanceArea = GetNode<Area2D>("Area2D");

		var collider = _avoidanceArea.GetChild<CollisionShape3D>(0).Shape as SphereShape3D;
		collider.Radius = AvoidanceDistance;
	}

	public override void _Process(double delta)
	{
		var targetPosition = AI.BehaviourTick(BehaviourComponent.Character,this);

		targetPosition = AvoidFilter(targetPosition);

		BehaviourComponent.Character.GetComponent<MovementControllerComponent>().Target = targetPosition;
		//TODO: add human like delay to movements to simulate reaction time. 

	}

	//TODO: Maybe look into moving the filter to the external behaviour to be adaptive to the situation
	Vector2 AvoidFilter(Vector2 targetLocation)
	{
		var insideBodies = _avoidanceArea.GetOverlappingBodies().Where(npc => npc != base.BehaviourComponent.Character);

		if (!insideBodies.Any())
			return targetLocation;

		var relativePositions = insideBodies.Select(body => body.GlobalPosition - BehaviourComponent.Character.GlobalPosition);

		Vector2 sum = Vector2.Zero;
		foreach (var position in relativePositions)
		{
			sum += position;
		}

		var relEnemyPosition = sum / insideBodies.Count();

		var character = BehaviourComponent.Character;

		var averageEnemyPosition = character.GlobalPosition + relEnemyPosition;

		//Opposite of averageEnemyPosition, best direction(not normalized) to get away from enemies, 
		var avoidPosition = ((character.GlobalPosition - averageEnemyPosition).Normalized() * AvoidanceDistance) + character.GlobalPosition;

        //Takes the average position of the enemies, and weights on depending on the distance to the npc
        //Nearer the average position is to the NPC more the NPC weights the direction to get away from enemies
        return ((avoidPosition - targetLocation) * (1 - ((averageEnemyPosition - character.GlobalPosition).Length() / AvoidanceDistance))) + targetLocation;
    }
}
