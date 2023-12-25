using Godot;
using Godot.Collections;
using Vain.SpellSystem;

namespace Vain.InteractionSystem;

[GlobalClass]
public partial class CharacterInfo : Resource
{
    [Export]
    public string Name {get;set;}
    [Export]
    public string Description {get;set;}


}
