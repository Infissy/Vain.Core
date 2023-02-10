using Godot;

namespace Vain
{
    [Singleton]
    public partial class MainCamera : Camera3D
    {
        
        
        [Export]
        ushort _raycastRange = 100;

        Player _player;        
        Vector3 _oldPlayerPosition;


        public override void _Ready()
        {
            base._Ready();
                
            _player = SingletonManager.GetSingleton<Player>();
            _oldPlayerPosition = _player.GlobalPosition;

   
        }   




        public override void _Process(double delta)
        {
            base._Process(delta);
            
            var relMotion =  _player.GlobalPosition - _oldPlayerPosition;
        

            this.GlobalTranslate(relMotion);
            //TODO: Add smoothing and eventually clipping
            _oldPlayerPosition = _player.GlobalPosition; 

        }

        
    
        public Vector3 GetMouseScenePosition()
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
                    
                    return target;
                    
                }
                else
                {
                    return Vector3.Inf;
                }
            
        
        }

     
    }
}