using Godot;
using Godot.Collections;

namespace Vain.Core;

[GlobalClass]
[Tool]
public partial class IndexResource : Resource
{
    [Export]
    public Dictionary<string, GodotObject> IndexedEntities {get;set;}
}
