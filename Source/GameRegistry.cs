using Godot;
using Vain.Core.ComponentSystem;
using Vain.Singleton;

namespace Vain.Core
{
    public partial class GameRegistry : Node
    {
        public IndexResource EntityIndex {get;set;} 
        public IndexResource ComponentIndex {get;set;} 
        public IndexResource BehaviourIndex {get;set;} 
        
        public override void _Ready()
        {
            base._Ready();

            EntityIndex = ResourceLoader.Load<IndexResource>("res://Resources/Game/EntityIndex.tres");
            ComponentIndex = ResourceLoader.Load<IndexResource>("res://Resources/Game/ComponentIndex.tres");
            BehaviourIndex = ResourceLoader.Load<IndexResource>("res://Resources/Game/BehaviourIndex.tres");


            
            SingletonManager.Register<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY,this);
        }
    }
}