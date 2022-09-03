using Godot;
using System.Collections.Generic;
using System.Linq;


//Not workign



//Component that enables entity to cast spells



namespace Vain.SpellSystem
{
    public abstract class SpellCaster : Component, IKillListenable, IInitalizable
    {

        public struct SpellsCastCount {
            public Spell Spell;
            public int CastCount;
        }
        
        
        Movable _movable;

        Dictionary< int , SpellsCastCount > _spells = new Dictionary<int, SpellsCastCount>();

        bool _changedSpells;

        [EditableField]
        SpellChanneler[] _spellChannelers = new SpellChanneler[10];


        protected int SpellCount{get {return _spellChannelers.Length;} }

        public Dictionary< int , SpellsCastCount > Spells 
        {
            
            get 
            {
                if(_changedSpells) 
                    UpdateDictionary();
                
                _changedSpells = false;
                
                return _spells;
            
            }
        }


        public void Initialize()
        {
            
            _movable = GetComponent<Movable>();

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

        public void CastSpell(int index, Vector3 target){
            
            if(_spellChannelers[index] != null  && _spellChannelers[index].NumberOfCasts > 0)
            {
                
                var spellInstance  = _spellChannelers[index].InstantiateSpell() as Entity;
                


                //TODO: Better hierarchy, this should not be a component child
                this.AddChild(spellInstance);

                bool successfulCast = spellInstance.GetComponent<SpellBehaviour>().Cast(base.Entity, target);
                

                if(successfulCast){
                    
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

            
            foreach (SpellChanneler channeler in _spellChannelers)
            {
                if(channeler != null){
                    

                  
                    var spellDropInstance = channeler.InstantiateSpellDrop();

                    //! Find better place to store spells
                    Entity.GetTree().Root.AddChild(spellDropInstance);



                    var spellDrop = spellDropInstance as Entity;

                
                    var drop_movable = spellDrop.GetComponent<Movable>();
                    drop_movable.ForcePosition(_movable.Position);
                    spellDrop.GetComponent<SpellDrop>().SpellChanneler = channeler;

                }
                
            }
            
            base.Entity.Kill();
            
        }

        
        







        void collisionHandler(object sender, CollisionEventArgs args)
        {       
            
            if(args.Collider is Entity entity )
            {

                var spellDrop = entity.GetComponent<SpellDrop>(true);

                
                if(spellDrop != null)
                    AddSpell(spellDrop.SpellChanneler);
            }
        }




    }




}
