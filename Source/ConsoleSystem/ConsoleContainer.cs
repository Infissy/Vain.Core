using Godot;

public partial class ConsoleContainer : VBoxContainer{

    bool visible = false;
    


    public override void _Ready()
    {
        base._Ready();

        
        this.Hide();
    
    }


    public override void _UnhandledInput(InputEvent @event)
        {

            
            base._UnhandledInput(@event);

            if (@event is InputEventKey){
                var keyevent = @event as InputEventKey;
                if(keyevent.Pressed  &&  keyevent.Keycode  == Key.Backslash ){
                }


            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (!Input.IsActionJustPressed("ui_console"))
                return;

    
            if(!visible)
                Show();
            else
                Hide();

            visible = !visible;

        }
}