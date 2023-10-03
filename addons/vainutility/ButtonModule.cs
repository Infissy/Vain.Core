#if TOOLS
using Godot;

namespace Vain.Plugins.VainUtility
{
    internal abstract partial class ButtonModule : Module
    {
        const string BUTTON_SCENE_PATH = "res://addons/vainutility/Resources/ButtonModule.tscn";
        Button _button;

        [Signal]
        internal delegate void PressedEventHandler();

        
        protected virtual string ButtonName 
        {
            get => _button.Text;
            set => _button.Text = value;
        }
        public override void _Ready()
        {
            base._Ready();
            this.CustomMinimumSize = new Vector2(200,40);

            _button = ResourceLoader.Load<PackedScene>(BUTTON_SCENE_PATH).Instantiate() as Button;
            _button.Pressed += () => EmitSignal(SignalName.Pressed);
            AddChild(_button);

        }


    }
}

#endif