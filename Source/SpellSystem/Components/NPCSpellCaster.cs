using Godot;

using Vain.Core;
using Vain.Core.ComponentSystem;

namespace Vain.SpellSystem;

partial class NPCSpellCaster : SubBehaviour
{


    [Export]
    public float ChanceToCast { get; set; }
    [Export]
    public float AggressionLevel {get;set;}

    public Character HostileCharacter {get;set;}


    public override void _Process(double delta)
    {
        base._Process(delta);

    }

}