#if TOOLS
using Godot;

namespace Vain.Plugins.VainUtility
{
    internal partial class SubDock : VBoxContainer
    {
        string ModuleName 
        {   
            get => GetChild<Label>(0).Text;
            set => GetChild<Label>(0).Text = value;
        }


        public override void _EnterTree()
        {
            base._EnterTree();

            
            AddChild(new Label());    
            AddChild(new VBoxContainer());
        }


        public void AddToModule(Node node)
        {
            GetChild(1).AddChild(node);
        }
    } 
}
#endif