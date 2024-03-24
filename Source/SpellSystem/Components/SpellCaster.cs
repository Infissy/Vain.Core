using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using Vain.Core;

using Vain.Core.ComponentSystem;
using Vain.HubSystem;
using static Vain.HubSystem.GameEvent.GameEvents;

namespace Vain.SpellSystem;

//Component that enables a character to cast spells
public abstract partial class SpellCaster : Component
{

	[Export]
	public Array<SpellChanneler> SpellChannelersExport {get;set;} = new Array<SpellChanneler>();

	public List<SpellChanneler> SpellChannelers {get; private set;} =  new List<SpellChanneler>();


	[Signal]
	public delegate void SpellPickupEventHandler();
	[Signal]
	public delegate void SpellCastEventHandler();

	public override void _Ready()
	{
		base._Ready();

		Character.CharacterKilled += OnKill;

		//TODO: Can't export list, and array are bothersome, fix until you can export c# collections
		foreach (var spellChanneler in SpellChannelersExport)
		{
			SpellChannelers.Add(spellChanneler?.Duplicate() as SpellChanneler);
		}

	}

	public bool AddSpell(SpellChanneler channeler){


		for (int i = 0; i < SpellChannelers.Count; i++)
		{
			if (SpellChannelers[i] == null)
			{
				SpellChannelers[i] = channeler;

				Hub.Instance.Emit<SpellPickupEvent,SpellPickupEventArgs>(new SpellPickupEventArgs{ Caster = this, Spell = channeler });
				EmitSignal(SignalName.SpellPickup);
				return true;
			}
		}

		return false;


	}


	public void AddSpells(IEnumerable<SpellChanneler> channelers){
		foreach(SpellChanneler channeler in channelers){
			//TODO: Duplication to avoid weird resource handling, look into it since it can cause weird behaviour? 
			AddSpell(channeler as SpellChanneler);
		}
	}

	public void CastSpell(int index, Vector2 target){

		if(SpellChannelers[index]?.CastCount == 0)
			return;

		EmitSignal(SignalName.SpellCast,SpellChannelers[index]);

		var spellCasted = SpellChannelers[index].CastSpell(base.Character,target);

		if(spellCasted && SpellChannelers[index].CastCount <= 0)
			SpellChannelers[index] = null;

	}


	void OnKill()
	{

		foreach (SpellChanneler channeler in SpellChannelers)
		{
			if(channeler == null)
				continue;


			var spellDropInstance = channeler.DropSpell();

			//! Find better place to store spells
			GetParent().GetParent().AddSibling(spellDropInstance);



			var spellDrop = spellDropInstance as SpellDrop;

			spellDrop.Translate(this.Character.GlobalPosition);
			spellDrop.SpellChanneler = channeler;

		}

	}


}
