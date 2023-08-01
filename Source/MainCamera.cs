using Godot;

using Vain.Singleton;
namespace Vain.Core
{

    /// <summary>
    /// Main Game Camera
    /// </summary>
    
    public partial class MainCamera : Camera3D
    {
        
        /// <summary>
        /// Character to follow.
        /// </summary>

        public Character? Player {get;set;}        
        
    
        /// <summary>
        /// Max Depth in which the camera can sample a mouse position in the scene. 
        /// </summary>
        [Export]
        public ushort RaycastRange {get;set;} = 100;



        Vector3 _oldPlayerPosition;



        public override void _EnterTree()
        {
            base._EnterTree();
            SingletonManager.Register(SingletonManager.Singletons.MAIN_CAMERA,this);
        }


        public override void _Ready()
        {
            base._Ready();
            
            Player = SingletonManager.GetSingleton<Character>(SingletonManager.Singletons.PLAYER);

            if(Player == null)
                throw new RequiredSingletonMissingException(SingletonManager.Singletons.PLAYER,typeof(Character));

            _oldPlayerPosition = Player.GlobalPosition;

   
        }   

        public override void _Process(double delta)
        {
            base._Process(delta);
            
            var relMotion =  Player.GlobalPosition - _oldPlayerPosition;
        

            this.GlobalTranslate(relMotion);
            //TODO: Add smoothing and eventually clipping
            _oldPlayerPosition = Player.GlobalPosition; 

        }

        
        /// <summary>
        /// Get the mouse position in the scene.
        /// The position is based on raycasting so it will always be on the underlying geometry.  
        /// </summary>
        public Vector3 GetMouseScenePosition()
        {
        
        
            var mousePos = GetViewport().GetMousePosition();
            var from = base.ProjectRayOrigin(mousePos);
            var to = from + base.ProjectRayNormal(mousePos) * RaycastRange;
                

                var space = base.GetWorld3D().DirectSpaceState;

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