#if TOOLS
using Godot;
using System;

namespace Vain.InteractionSystem.InteractionGraph
{
    
    [Tool]
    public partial class InteractionGraph : EditorPlugin
    {
     
        Control dock;

        public override void _EnterTree()
        {

       
            dock = GD.Load<PackedScene>("res://addons/interactiongraph/Nodes/MainGraph.tscn").Instantiate<Control>();

           
            GetEditorInterface().GetEditorMainScreen().AddChild(dock);
            _MakeVisible(false);
        }

        public override void _ExitTree()
        { 
         
            dock?.QueueFree();
            
        }

        

        public override bool _HasMainScreen()
        {
            return true;
        }

        public override void _MakeVisible(bool visible)
        {
            if (dock != null)
            {
                dock.Visible = visible;
            }
        }

        public override string _GetPluginName()
        {
            return "Interaction Graph";
        }

        public override Texture2D _GetPluginIcon()
        {
            // Must return some kind of Texture for the icon.
            return GetEditorInterface().GetBaseControl().GetThemeIcon("Node", "EditorIcons");
        }

    }
}
#endif
