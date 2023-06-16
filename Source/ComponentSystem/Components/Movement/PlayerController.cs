using System;
using Godot;
using Vain.Core;
using Vain.Log;

namespace Vain
{
	

	public partial class PlayerController : CharacterController
	{
		
	 

	
		public override void _UnhandledInput(InputEvent inputEvent)
		{
		
			
			base._UnhandledInput(inputEvent);

			
			if (inputEvent is InputEventMouseButton){
				var mouseEvent = inputEvent as InputEventMouseButton;
				if(mouseEvent.IsPressed() && mouseEvent.ButtonIndex ==  MouseButton.Right)
				{
				

					var target = SingletonManager.GetSingleton<MainCamera>().GetMouseScenePosition();
					if(target != Vector3.Inf)
						EmitSignal(SignalName.MovementInput,target);
					
				}



			}


		}
	}


}
