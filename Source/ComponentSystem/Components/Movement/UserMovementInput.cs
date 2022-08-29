using System;
using Godot;
using Vain.Log;

namespace Vain
{
    

    public class UserMovementInput : Component
    {
        
     

        Movable _movable;


        public override void _Ready()
        {
            base._Ready();


            _movable = GetParent<ComponentContainer>().GetComponent<Movable>();

            
        }

        public override void _UnhandledInput(InputEvent inputEvent)
        {
            base._UnhandledInput(inputEvent);


            if (inputEvent is InputEventMouseButton){
                var mouseEvent = inputEvent as InputEventMouseButton;
                if(mouseEvent.IsPressed() &&mouseEvent.ButtonIndex ==  2)
                {

                    var target = MainCamera.Instance.MouseTargetInScene;
                    if(target != Vector3.Inf)
                        _movable.Target = target;
                    
                }



            }


        }
    }


}
