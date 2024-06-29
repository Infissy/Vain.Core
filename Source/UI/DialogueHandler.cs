using Godot;
using Vain.Core;
using Vain.InteractionSystem;

using Vain.Singleton;


namespace Vain.UI.DialogueSystem;

//TODO: At the moment it doesn't handle character switching, fix it if necessary
public partial class DialogueHandler : Control
{
    [Export]
    PackedScene _dialogueBox;


    


    void HandleDialogue(string dialogue)
    {
        if(GetChildCount() > 0)
            GetChild(0).QueueFree();


        var dialogueBox = _dialogueBox.Instantiate<DialogueBox>();

        AddChild(dialogueBox);

        dialogueBox.Dialogue = dialogue;

    }

}
