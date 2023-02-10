using Godot;

namespace Vain.InteractionSystem
{
    partial class SimpleDialogueInteraction : SimpleInteraction, DialogueInteraction
    {
        [Export]
        public string Dialogue {get; set;}
    }

}