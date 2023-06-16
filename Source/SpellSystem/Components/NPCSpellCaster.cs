using Godot;
using System;
using System.Collections.Generic;
using Vain.Core;



namespace Vain.SpellSystem
{
	partial class NPCSpellCaster : SpellCaster 
	{


		[Export]
		public float ChanceToCast { get; set; }
     

		

        public override void _Process(double delta)
		{
			base._Process(delta);






		    if ((Character as Enemy).HostileTarget != null)
               castTick(delta);
            


        }


        //TODO: Output message when spellcaster has 0 slots available
        void castTick(double delta)
        {
            var npc = Character as NPC;

            if(npc.AggressionLevel < npc.HostileTarget.GlobalPosition.DistanceTo(npc.GlobalPosition))
                return;



            if (GD.Randf() < ChanceToCast * delta)
            {
                int spellindex = GD.RandRange(0, SpellChannelers.Count - 1);

                if (SpellChannelers[spellindex] != null && SpellChannelers[spellindex].Spell != Spell.NONE)
                {


                    base.CastSpell(spellindex, (Character as Enemy).HostileTarget.Position);
                    

                }
            }
        }



		
		
	}

}
