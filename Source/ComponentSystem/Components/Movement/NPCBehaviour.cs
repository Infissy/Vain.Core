using Godot;
using Vain.Core;
namespace Vain
{
    public abstract partial class NPCBehaviour : Resource
    {
        public abstract Vector3 BehaviourTick(NPC character);
    }
}