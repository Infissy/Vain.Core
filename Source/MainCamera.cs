using Godot;
namespace Vain

{
        
    public class MainCamera : Camera  {
        
        

        public static MainCamera Instance;
        
        public override void _Ready(){
            base._Ready();
            Instance = this;
        }
    }
}