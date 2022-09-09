using System;
using Godot;
using Vain.Log;

namespace Vain
{
	

	public class UserMovementInput : Component, IInitialize, IUnhandledInputHandler
	{
		
	 

		Movable _movable;


		public void Initialize()
		{

			_movable = GetComponent<Movable>();

			
		}

		public void UnhandledInput(InputEvent inputEvent)
		{
		


			if (inputEvent is InputEventMouseButton){
				var mouseEvent = inputEvent as InputEventMouseButton;
				if(mouseEvent.IsPressed() && mouseEvent.ButtonIndex ==  2)
				{
				

					var target = MainCamera.Instance.MouseTargetInScene;
					if(target != Vector3.Inf)
						_movable.Target = target;
					
				}



			}


		}
	}


}
