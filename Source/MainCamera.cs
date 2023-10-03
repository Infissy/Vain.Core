using Godot;

using Vain.Singleton;
using Vain.Log;



namespace Vain.Core
{

    /// <summary>
    /// Main Game Camera
    /// </summary>
    
    public partial class MainCamera : Camera2D
    {
        
        /// <summary>
        /// Character to follow.
        /// </summary>

        public Singleton<Character>? Player {get;set;}        
        
    
        /// <summary>
        /// Max Depth in which the camera can sample a mouse position in the scene. 
        /// </summary>
        [Export]
        public ushort RaycastRange {get;set;} = 500;



        Vector2 _oldPlayerPosition;



        public override void _EnterTree()
        {
            base._EnterTree();
            SingletonManager.Register(SingletonManager.Singletons.MAIN_CAMERA,this);
        }


        public override void _Ready()
        {
            base._Ready();
            


            Player = SingletonManager.GetSingleton<Character>(SingletonManager.Singletons.PLAYER, ()=>{});


            if(Player.Reference != null)
                _oldPlayerPosition = Player!.Reference!.GlobalPosition;

   
        }   

        public override void _Process(double delta)
        {
            base._Process(delta);
            
            if(Player.Reference == null)
                return;

            var relMotion =  Player!.Reference!.GlobalPosition - _oldPlayerPosition;
        

            this.GlobalTranslate(relMotion);
            //TODO: Add smoothing and eventually clipping
            _oldPlayerPosition = Player.Reference.GlobalPosition; 

        }

        
        /// <summary>
        /// Get the mouse position in the scene.
        /// The position is based on raycasting so it will always be on the underlying geometry.  
        /// </summary>
        public Vector2 GetMouseScenePosition()
        {
            return GetGlobalMousePosition();
        }


     
    }
}