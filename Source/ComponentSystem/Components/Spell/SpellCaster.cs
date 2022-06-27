using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Vain.SpellSystem
{
    public class SpellCaster : Component, IKillListenable
    {

        public struct SpellsCastCount {
            public Spell Spell;
            public int CastCount;
        }
        
        
        Movable _movable;
        Dictionary< int , SpellsCastCount > _spells = new Dictionary<int, SpellsCastCount>();
        bool _changedSpells;









        
        [Export]
        SpellChanneler[] _spellChannelers = new SpellChanneler[10];









        protected int SpellCount{get {return _spellChannelers.Length;} }


        public Dictionary< int , SpellsCastCount > Spells {
            
            get 
            {
                if(_changedSpells) 
                    UpdateDictionary();
                
                _changedSpells = false;
                
                return _spells;
            
            }
        }


        public override void _Ready()
        {
            base._Ready();

            _movable = ComponentEntity.ComponentContainer.GetComponent<Movable>();

            _movable.OnCollision += collisionHandler;
        }


        

        public void AddSpell(SpellChanneler channeler){

            bool found = false;
            for (int i = 0; i < _spellChannelers.Length; i++)
            {
                if(_spellChannelers[i] != null && _spellChannelers[i].Spell == channeler.Spell){
                    
                    _spellChannelers[i].NumberOfCasts += channeler.NumberOfCasts;

                    found = true;
                    break;
                }
            }

            if(!found){
                for (int i = 0; i < _spellChannelers.Length; i++)
                {
                    if(_spellChannelers[i] == null){
                        _spellChannelers[i] = channeler;
                        break;
                    }
                }
            }

        

            _changedSpells = true;
            //If player then refresh ui
        }
        

        public void AddSpells(List<SpellChanneler> channelers){
            foreach(SpellChanneler channeler in channelers){
                AddSpell(channeler);
            }
        }

        public void CastSpell(int index, Vector2 target){
            
            if(_spellChannelers[index] != null)
            {

                var spellInstance  = _spellChannelers[index].SpellPrefab as Entity;
                
                this.AddChild(spellInstance);

                bool successfulCast = spellInstance.ComponentContainer.GetComponent<SpellBehaviour>().Cast(ComponentEntity, target);
                

                if(successfulCast){
                    _spellChannelers[index].NumberOfCasts--;  
                    if(_spellChannelers[index].NumberOfCasts == 0){
                        _spellChannelers[index] = null;

                        _changedSpells = true;

                        //ChangeUI
                    }

                    UpdateDictionary();
                }
            
            }
            



            
        }


        public void UpdateDictionary(){
            
            
            _spells = new Dictionary<int, SpellsCastCount>();
            
            int index = 0;
            
            foreach(SpellChanneler channeler in _spellChannelers){
                SpellsCastCount sp;
                if(channeler != null){
                    sp = new SpellsCastCount{Spell = channeler.Spell, CastCount = channeler.NumberOfCasts};

                } else{
                    sp = new SpellsCastCount{Spell = Spell.None,CastCount = 0};
                }


                _spells.Add(index++,sp);

            }
                
        }

        
        public void OnKill()
        {
            ComponentEntity.QueueFree();

            
            foreach (SpellChanneler channeler in _spellChannelers)
            {
                if(channeler != null){
                    var spellDrop = channeler.SpellDropPrefab as Entity;
                    spellDrop.Position = ComponentEntity.Position - spellDrop.Position;
                    spellDrop.ComponentContainer.GetComponent<SpellDrop>().SpellChanneler = channeler;

                }
                
            }
            
            
        }

        
        







        void collisionHandler(Node collider)
        {
            if(collider is SpellDrop spellDrop)
            {
                AddSpell(spellDrop.SpellChanneler);
            }
        }




    }




}
