
using Godot;
using Godot.Collections;

//Not working

namespace Vain.SpellSystem
{

    public abstract partial class SpellInstance : Area3D
    {

        
        
        
        [Export]
        public float LifeTime { get; protected set;}
        




  
        [Export]

        //TODO: Find a better way to visualize the resource in the editor
        protected Array<Effect> effects =  new Array<Effect>();


        
        [Export]
        public SpellChanneler NextSpell {get; set;}
        

        public Character Caster{get; set;}

    

        internal abstract bool Perform(Character owner, Vector3 target);
        
      
        
    }
}