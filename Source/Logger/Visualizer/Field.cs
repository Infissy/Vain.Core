using Godot;

namespace Vain.Log.Visualizer;

//TODO: Add formatting options
internal partial class Field : HBoxContainer
{
    string _fieldName;
    public string Value
    {
        get => _valueLabel.Text;
        set
        {
            _valueLabel.Text = value;
        }
    }

    Label _valueLabel;

    public Field(string name)
    {
        _fieldName = name;
        _valueLabel = new Label();
    }

    public override void _Ready()
    {
        var nameLabel = new Label
        {
            Text = _fieldName
        };


        AddChild(nameLabel);

        AddChild(_valueLabel);
    }

}
