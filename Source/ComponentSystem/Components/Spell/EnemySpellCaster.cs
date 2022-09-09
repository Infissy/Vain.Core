using Godot;
using System;
using System.Collections.Generic;
using Vain;




namespace Vain.SpellSystem

{
    class EnemySpellCaster : SpellCaster, IProcess
    {

        Dictionary< int, SpellsCastCount> _spells;
        
        public float ChanceToCast {get;set;}
        
        public void Process(float delta)
        {
         
            _spells = Spells;
        
            if(  GD.Randf() / delta < ChanceToCast && _spells.Count > 0){

                int spellindex= (int) GD.RandRange(0,_spells.Count-1);

                if(_spells[spellindex].Spell != Spell.None){
                    CastSpell(spellindex,Player.Instance.Entity.GetComponent<Movable>().Position);

                }
            }
        }



        
        
    }

}