using Godot;
using Godot.Collections;

namespace Vain.Core
{
    [GlobalClass]
    public partial class EntityIndexResource : Resource
    {
        [Export]
        public Dictionary<string, GodotObject> Entities {get;set;}
    }
}