using Godot;

using Vain.Singleton;


namespace Vain.Core.ComponentSystem
{
    [GlobalClass]
    /// <summary>
    /// Required to register the current character as the player
    /// </summary>
    public partial class PlayerEntitySubBehaviour : SubBehaviour
    {
        public override void _Ready()
        {
            base._Ready();
            SingletonManager.Register<Character>(SingletonManager.Singletons.PLAYER,BehaviourComponent.Character);
        }
        
    }
}