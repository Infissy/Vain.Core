using Godot;
using Godot.Collections;


namespace Vain.Core.ComponentSystem
{
    [GlobalClass]
    public partial class ComponentIndexResource : Resource
    {
        [Export]
        public Dictionary<string, GodotObject> Components;
    }
}