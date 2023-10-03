using Godot;


using Vain.Core.ComponentSystem;

namespace Vain.InteractionSystem
{

    
    public partial class InteractibleComponent : Component
    {
        [Export]
        Interaction _interaction;


        public Interaction Interact()
        {

            return _interaction;
        }

        
        
    }
}