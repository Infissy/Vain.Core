using System;
using Godot;
using Vain.Core;
using Vain.Log;
using Vain.Singleton;
namespace Vain.Core.ComponentSystem.Behaviour
{
	

	public partial class PlayerMovementSubBehaviour : SubBehaviour 
	{
		
	 

	
		public override void _Process(double delta)
		{
			base._Process(delta);
			
			if(!Input.IsActionJustPressed("player_movement_input"))
				return;

		
			var target = SingletonManager.GetSingleton<MainCamera>(SingletonManager.Singletons.MAIN_CAMERA).Reference.GetMouseScenePosition();


			if(target == Vector2.Inf)
				return;


			var controller = base.BehaviourComponent.Character.GetComponent<MovementControllerComponent>();
			
			if(controller == null)
			{
				Logger.GlobalLogger.SetContext(this).Critical("Player movements needs a CharacterController in order to work properly, please add it to the entity");
				return;
			}

			controller.Target = target;
			


		}
	}


}
