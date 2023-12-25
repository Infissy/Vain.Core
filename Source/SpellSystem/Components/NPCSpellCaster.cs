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
        if (HostileCharacter == null)
            return;

            if(AggressionLevel < HostileCharacter.GlobalPosition.DistanceTo(BehaviourComponent.Character.GlobalPosition))
            return;


        var spellCasterComponent = BehaviourComponent.Character.GetComponent<SpellCaster>();
        var spellChannelers = spellCasterComponent.SpellChannelers;

        if (GD.Randf() < ChanceToCast * delta)
        {
            int spellindex = GD.RandRange(0, spellChannelers.Count - 1);

            if (spellChannelers[spellindex] != null)
            {
                spellCasterComponent.CastSpell(spellindex, HostileCharacter.Position);
            }
        }


    }

}