using Godot;

namespace Vain.SpellSystem;

public abstract partial class Spell : Resource
{
    [Export]
    public int CastCount;
}
