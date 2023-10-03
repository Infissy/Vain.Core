#if TOOLS

using Godot;

namespace Vain.Plugins.VainUtility
{
    internal abstract partial class ResourceSelectionModule : Module
    {
        const string FILE_SELECTION_SCENE_PATH = "res://addons/vainutility/Resources/ResourceSelectionModule.tscn";
        Button _button;
        EditorResourcePicker _resourcePicker;

        [Signal]
        internal delegate void FileSelectedEventHandler(Resource path);

        
        protected virtual string ButtonName 
        {
            get => _button.Text;
            set => _button.Text = value;
        }


        protected virtual string SelectedFilePath {get;private set;}
        public override void _Ready()
        {
            base._Ready();

            this.CustomMinimumSize = new Vector2(400,40);

            SizeFlagsHorizontal = SizeFlags.Expand;
            var module = ResourceLoader.Load<PackedScene>(FILE_SELECTION_SCENE_PATH).Instantiate() as Control;
           
            
            _resourcePicker = module.GetChild<EditorResourcePicker>(0);
            _resourcePicker.BaseType = "CSharpScript,PackedScene";

            _button = module.GetChild<Button>(2);
            _button.Pressed += () => EmitSignal(SignalName.FileSelected,_resourcePicker.EditedResource);
            AddChild(module);

        }


    }
}

#endif