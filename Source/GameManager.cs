using Godot;

using Vain.InteractionSystem;

using Vain.Singleton;

namespace Vain.Core
{

    
    

    //TODO: ? maybe rename, bad name
    
    /// <summary>
    /// Highest parent of the game SceneTree, stores and handles data between scenes (Maps/Menus).
    /// </summary>
    public partial class GameManager : Node
    {  
        [Signal]
        public delegate void SceneChangeEventHandler();



        public override void _Ready()
        {
            
            SingletonManager.GetSingleton<Player>().PlayerDeath += () =>
            {
                
                GetTree().ReloadCurrentScene();
                EmitSignal(SignalName.SceneChange);


            };
                
            
            
           
        }
        


    }
}