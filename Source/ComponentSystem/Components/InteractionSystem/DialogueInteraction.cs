using Godot;



namespace Vain.InteractionSystem;

[GlobalClass]
partial class DialogueInteraction : Interaction
{   
    [Export]
    public string Dialogue {get; set;}    
}
