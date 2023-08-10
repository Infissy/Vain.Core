using Godot;

using Vain.Singleton;


namespace Vain.Core.ComponentSystem
{
    [GlobalClass]
    public partial class PlayerBehaviour : CharacterBehaviour
    {
        public override void _Ready()
        {
            SingletonManager.Register<Character>(SingletonManager.Singletons.PLAYER,this.Character);
        }
        
    }
}