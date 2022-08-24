using System;
using System.Collections.Generic;
using Godot;

//Not working

namespace Vain.SpellSystem
{

    public abstract class SpellBehaviour : Component
    {

        
        
        


        [Export(PropertyHint.MultilineText)]
        protected string behaviour;
    




        [Export] 
        protected float Lifetime;
  
        protected List<Effect> effects = new List<Effect>();
        protected bool EffectFetched = false;


        public List<Effect> Effects {get{
            EffectFetched = true;
            
            return effects;
            
        }}


       

        public abstract bool Cast(Entity owner, Vector2 target);
        
        
        
    }
}