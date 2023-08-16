using Godot;

namespace Vain.Core.ComponentSystem
{
    public abstract partial class SubBehaviour : Node
    {
        protected CharacterBehaviourComponent BehaviourCluster {get;private set;}
        public override void _Ready()
        {
            BehaviourCluster = GetParent<CharacterBehaviourComponent>();
        }
    }
}