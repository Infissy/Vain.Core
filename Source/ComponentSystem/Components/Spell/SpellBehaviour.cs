using System;
using System.Collections.Generic;
using Godot;

//Not working

namespace Vain.SpellSystem
{

    public abstract class SpellBehaviour : Component
    {

        
        
        

        




        [Export] 
        protected float Lifetime;
  
        protected List<Effect> effects = new List<Effect>();
        protected bool EffectFetched = false;


        public List<Effect> Effects 
        {
            get{
                
                
                EffectFetched = true;
            
                return effects;
            
            }
        }


       

        public abstract bool Cast(Entity owner, Vector3 target);
        
        
        
    }
}