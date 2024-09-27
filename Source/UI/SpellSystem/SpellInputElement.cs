

using Godot;

namespace Vain.SpellSystem.UI;


public partial class SpellInputElement : Control
{
    static PackedScene s_inputScene = ResourceLoader.Load<PackedScene>("res://Vain.Core/Prefabs/UI/SpellSystem/SpellInput.tscn");

    public SpellInputElement(SpellInputType type)
    {
        _inputType = type;
    }


    SpellInputType _inputType;
    public SpellInputType InputType
    {
        get => _inputType;

        set
        {
            _inputType = value;

            foreach (Control child in GetChildren())
            {
                child.Visible = false;


            }
            (GetChild((int)_inputType) as Control).Visible = true;

        }
    }

    bool _pressed = false;

    public bool Pressed
    {
        get => _pressed;
        set
        {
            _pressed = value;
            (GetChild((int)_inputType) as Control).Modulate = Colors.White;
        }
    }



    public enum SpellInputType
    {
        Up,
        Down,
        Left,
        Right
    }


    public override void _Ready()
    {
        base._Ready();
        var scene = s_inputScene.Instantiate();


        //TODO: Add mechanism for scene instantiation
        foreach (Control child in scene.GetChildren())
        {
            child.Owner = null;
            child.Reparent(this);
            child.Visible = false;
            child.Modulate = new Color(1, 1, 1, 0.5f);
        }

        InputType = InputType;
    }

};






