using Godot;


namespace Vain.Console;


public partial class ConsoleContainer : VBoxContainer{

    bool _visible;

    public override void _Ready()
    {
        base._Ready();

        Hide();
    }



    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!Input.IsActionJustPressed("ui_console"))
            return;


        if(!_visible)
            Show();
        else
            Hide();

        _visible = !_visible;

    }
}

