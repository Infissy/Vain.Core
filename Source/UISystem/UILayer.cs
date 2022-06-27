using Godot;
using Vain.UI;
public class UILayer : CanvasLayer
{
    UI ui;

    public override void _Ready(){

        var control = new Control();

        this.AddChild(control);


        ui = new UI(control);
    
    
    }
} 