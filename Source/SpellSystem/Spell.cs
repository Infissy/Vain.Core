using Godot;



namespace Vain.SpellSystem.New
{
    abstract partial class Spell : Resource
    {
       [Export]
       public int CastCount; 
    }
}