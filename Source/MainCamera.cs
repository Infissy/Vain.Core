using Godot;
namespace Vain

{
        
    public class MainCamera : Godot.Camera2D
    {
        public Vector2 MousePosition => GetGlobalMousePosition();

        public static MainCamera Instance;
        
        public override void _Ready(){
            base._Ready();
            Instance = this;
        }
    }
}