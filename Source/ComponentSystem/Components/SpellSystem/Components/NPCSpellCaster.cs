using Godot;
using System;
using System.Collections.Generic;
using Vain;




namespace Vain.SpellSystem
{
	partial class NPCSpellCaster : SpellCaster 
	{


		[Export]
		public float ChanceToCast { get; set; }
     

		

        public override void _Process(double delta)
		{
			base._Process(delta);






			if ((Character as NPC).HostileTarget != null)
                castTick(delta);
            


        }


        //TODO: Output message when spellcaster has 0 slots available
        void castTick(double delta)
        {

            if (GD.Randf() < ChanceToCast * delta)
            {
                int spellindex = GD.RandRange(0, SpellChannelers.Count - 1);

                if (SpellChannelers[spellindex] != null && SpellChannelers[spellindex].Spell != Spell.NONE)
                {


                    base.CastSpell(spellindex, (Character as NPC).HostileTarget.Position);

                }
            }
        }



		
		
	}

}
