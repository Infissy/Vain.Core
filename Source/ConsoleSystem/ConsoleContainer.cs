using Godot;


namespace Vain.Console
{

    public partial class ConsoleContainer : VBoxContainer{

        bool visible = false;
        


        public override void _Ready()
        {
            base._Ready();

            
            this.Hide();
        
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

}