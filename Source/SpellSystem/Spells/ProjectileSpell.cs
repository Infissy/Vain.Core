using Godot;

namespace Vain.SpellSystem
{
    public abstract partial class ProjectileSpell : SpellInstance
    {
        



        [Export]
        public float Speed {get; protected set;}
        
        
        
        public Vector3 Direction{get; protected set;}
      
        [Export]
        public Vector3 Weight {get; protected set;}

    

        public override void _Process(double delta)
        {   
            base._Process(delta);


            GlobalTranslate((Direction + Weight).Normalized() * (float) delta * Speed);



            
            

        }
         
      

    }
}