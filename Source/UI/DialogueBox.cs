using Godot;

namespace Vain.UI.DialogueSystem;

public partial class DialogueBox : Control
{

    Label _textLabel;
    public string Dialogue
    {
        set
        {
            _textLabel.Text = value;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        _textLabel = GetNode<Label>("MarginContainer/Label");

    }
}
