using Godot;
using Vain.Singleton;

namespace Vain.Core
{
    public partial class GameRegistry : Node
    {
        public EntityIndexResource EntityIndex {get;set;} = ResourceLoader.Load<EntityIndexResource>("res://Resources/Game/EntityIndex.tres");

        public override void _Ready()
        {
            base._Ready();

            SingletonManager.Register<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY,this);
        }
    }
}