using System.Collections.Generic;
using Godot;

using Vain.Core.ComponentSystem;
using Vain.HubSystem;

using static Vain.HubSystem.GameEvent.GameEvents.Spell;
namespace Vain.SpellSystem;

//Component that enables a character to cast spells
public partial class SpellCasterComponent : Component
{

	public Dictionary<string, string[]> Spells { get; set; } = new Dictionary<string, string[]>();


	public void CastSpell(string spell, string template, Vector2 target)
	{
		Hub.Instance.Emit<SpellCastEvent, SpellCastEventArgs>(new SpellCastEventArgs{
			Caster = this.Character,
			SpellName = spell,
			Template = template,
			Target = target

		});
	}



}
