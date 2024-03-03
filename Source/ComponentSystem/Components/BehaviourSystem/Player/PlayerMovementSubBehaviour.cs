using System;
using System.Diagnostics;
using Godot;


using Vain.HubSystem;

using static Vain.HubSystem.Query.Queries;
namespace Vain.Core.ComponentSystem.Behaviour;

public partial class PlayerMovementSubBehaviour : SubBehaviour
{
	public override void _Process(double delta)
	{
		base._Process(delta);
		if(!Input.IsActionJustPressed("player_movement_input"))
			return;

		var targetResponse = Hub.Instance.QueryData<MousePositionQuery, EmptyQueryRequest,PositionQueryResponse>();

		if(targetResponse == null || targetResponse?.Position == Vector2.Inf)
			return;

		var controller = BehaviourComponent.Character.GetComponent<MovementControllerComponent>();

		Debug.Assert(controller != null, "MovementControllerComponent not found in Character");

		controller.Target = targetResponse?.Position ?? Vector2.Zero;
	}
}

