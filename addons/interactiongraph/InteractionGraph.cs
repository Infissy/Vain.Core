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
            dock = GD.Load<PackedScene>("res://addons/interactiongraph/MainGraph.tscn").Instantiate<Control>();

            dock.GetNode<Button>("VBoxContainer/Panel/HBoxContainer/Characters").Pressed += () => NodeFactory.GenerateCharacters();
            dock.GetNode<Button>("VBoxContainer/Panel/HBoxContainer/Interactions").Pressed += () => NodeFactory.GenerateInteractions();

            AddControlToBottomPanel(dock, "InteractionEdit");
            
            
        }

        public override void _ExitTree()
        {
            // Clean-up of the plugin goes here.
            // Remove the dock.
            RemoveControlFromBottomPanel(dock);
            // Erase the control from the memory.
            dock.QueueFree();
        }

    }
}
#endif
