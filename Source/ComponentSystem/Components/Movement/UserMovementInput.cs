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
                if(mouseEvent.IsPressed() &&mouseEvent.ButtonIndex ==  2){
                    
                    var camera = MainCamera.Instance;


                    var from = camera.ProjectRayOrigin(mouseEvent.Position);
                    var to = from + camera.ProjectRayNormal(mouseEvent.Position) * 100;
                    

                    var space = camera.GetWorld().DirectSpaceState;

                    var intersection = space.IntersectRay(from,to);

                    
                    if(intersection.Count > 0)
                    {
                        _movable.Target = (Vector3) intersection["position"] ;
                        
                        

                        //TODO: Make system for debug draw
                        
                        var d_node = new CSGSphere();
                        d_node.GlobalTranslate(_movable.Target);
                        d_node.Scale = Vector3.One * 0.1f;
                        this.Owner.AddChild(d_node);


                        
                                

                        Logger.SetContext(ComponentEntity).Debug($"User move input at {_movable.Target}" );
                    }



                }


            }
        }


    }
}