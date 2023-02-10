using Godot;

namespace Vain.InteractionSystem
{

    
    public partial class Interactible : Component
    {
        [Export]
        Interaction _interaction;


        public Interaction Interact()
        {

            

            return _interaction;
        }

        
        
    }
}