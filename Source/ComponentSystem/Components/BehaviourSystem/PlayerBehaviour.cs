using Godot;

using Vain.Singleton;


namespace Vain.Core.ComponentSystem
{
    [GlobalClass]
    public partial class PlayerBehaviour : CharacterBehaviourComponent
    {
        public override void _Ready()
        {
            SingletonManager.Register<Character>(SingletonManager.Singletons.PLAYER,this.Character);
        }
        
    }
}