using Godot;
namespace Vain
{
    

    public class Player : Component
    {
        
        public static Player Instance;

        public Entity Entity => ComponentEntity;

        public override void _Ready(){

            base._Ready();
            
            Instance = this;
        }
        


    }

}