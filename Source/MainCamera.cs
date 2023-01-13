using Godot;

namespace Vain
{
        
    public partial class MainCamera : Camera3D  
    {
        
        
        [Export]
        ushort _raycastRange = 100;

        public static MainCamera Instance;
        
        public override void _Ready(){
            base._Ready();
            Instance = this;
        }

    
        public Vector3 MouseTargetInScene{
            
            get 
            {
                var mousePos = GetViewport().GetMousePosition();
                var from = base.ProjectRayOrigin(mousePos);
                var to = from + base.ProjectRayNormal(mousePos) * _raycastRange;
                    

                    var space = base.GetWorld3d().DirectSpaceState;

                    PhysicsRayQueryParameters3D query = new PhysicsRayQueryParameters3D();
                    query.From = from;
                    query.To = to;
           

                    var intersection = space.IntersectRay(query);

                    
                    if(intersection.Count > 0)
                    {
                        
                        var target = (Vector3) intersection["position"] ;
                        

                        //TODO: Make system for debug draw
                        /*
                        var d_node = new CSGSphere3D();
                        this.Owner.AddChild(d_node);
                        d_node.GlobalTranslate(target);
                        d_node.Scale = Vector3.One * 0.1f;
                        */

                        
                                

                        return target;
                        
                    }
                    else
                    {
                        return Vector3.Inf;
                    }
                
            }
        }
    }
}