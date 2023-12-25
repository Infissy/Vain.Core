using System;
using System.Diagnostics;
using Godot;

using Vain.Singleton;

namespace Vain.Core.ComponentSystem.Behaviour;

public partial class PlayerMovementSubBehaviour : SubBehaviour
{
	Singleton<MainCamera> _camera = SingletonManager.GetSingleton<MainCamera>(SingletonManager.Singletons.MAIN_CAMERA);

	public override void _Ready()
	{
		base._Ready();
		_camera = SingletonManager.GetSingleton<MainCamera>(SingletonManager.Singletons.MAIN_CAMERA);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if(!Input.IsActionJustPressed("player_movement_input"))
			return;

		if(_camera.Disposed)
			return;

		var target = _camera?.Reference.GetMouseScenePosition();

		if(target == Vector2.Inf)
			return;

		var controller = BehaviourComponent.Character.GetComponent<MovementControllerComponent>();

		Debug.Assert(controller != null, "MovementControllerComponent not found in Character");

		controller.Target = target ?? BehaviourComponent.Character.GlobalPosition;
	}
}
