using System;
using Godot;
using Vain.Core;

namespace Vain.SpellSystem
{


    
    public partial class SpellChanneler : Resource 
    {



        
        

        [Export]
        PackedScene _spellPrefab;
        [Export]
        PackedScene _spellDropPrefab;


        [Export]
        public Spell Spell {get; private set;}


        [Export]
        int _castCount;
        








        public int CastCount 
        {
            get => _castCount;
                
            set
            {
                
                EmitChanged();
                _castCount = value;

            }
        }




        public bool CastSpell(Character caster, Vector3 target)
        {
            if(CastCount > 0)
            {

                var spellInstance = _spellPrefab.Instantiate<SpellInstance>();

                //TODO: Better physics handling, should be clearer on which layer is what

                
                
                //Choose which layer the spell is, not the best way to select it 
                spellInstance.CollisionMask = caster is Player ? SpellLayer.NPC : SpellLayer.PLAYER;


                spellInstance.Caster = caster;

                //TODO: Better hierarchy
                caster.GetNode("/root/Game").GetChild(0).AddChild(spellInstance);

                var success = spellInstance.Perform(caster, target);
                
                if(success)
                {
                    CastCount--;
                    
                }   
                
                
                return success;

            }
            else
            {
                return false;
            }




				

				
            

        }

     
        public Node DropSpell()
        {
            return _spellDropPrefab.Instantiate();
        }

        

        //TEMP: made for resource instantatiation

        public SpellChanneler Copy()
        {
            return new SpellChanneler()
            {
                Spell = this.Spell,
                _spellDropPrefab = this._spellDropPrefab,
                _spellPrefab = this._spellPrefab,
                _castCount = this._castCount
            };
        }

    }

}