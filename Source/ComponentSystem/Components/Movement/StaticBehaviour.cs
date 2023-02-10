using Godot;

namespace Vain
{
    public partial class StaticBehaviour : NPCBehaviour
    {
        public override Vector3 BehaviourTick(NPC character)
        {
            return character.GlobalPosition;
        }
    }
}