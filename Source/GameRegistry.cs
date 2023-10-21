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
        public IndexResource LevelIndex {get;set;} 
        public override void _Ready()
        {
            base._Ready();

            EntityIndex = ResourceLoader.Load<IndexResource>(ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.EntityIndex));
            ComponentIndex = ResourceLoader.Load<IndexResource>(ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.ComponentIndex));
            BehaviourIndex = ResourceLoader.Load<IndexResource>(ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.BehaviourIndex));
            LevelIndex = ResourceLoader.Load<IndexResource>(ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.LevelIndex));



            
            SingletonManager.Register(SingletonManager.Singletons.GAME_REGISTRY,this);
        }
    }
}