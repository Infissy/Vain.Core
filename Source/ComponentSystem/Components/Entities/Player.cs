using Godot;
namespace Vain
{
    

    public class Player : Component, IInitalizable
    {
        
        public static Player Instance;


        public void Initialize(){

            Instance = this;
        }
        


    }

}