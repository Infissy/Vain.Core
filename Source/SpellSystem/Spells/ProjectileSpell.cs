using Godot;

namespace Vain.SpellSystem
{
    public abstract partial class ProjectileSpell : SpellInstance
    {
        



        [Export]
        public float Speed {get; set;}
        [Export]
        public Vector2 Weight {get; set;}
        
        public Vector2 Direction {get; set;}
      

    

        public override void _Process(double delta)
        {   
            base._Process(delta);
            GlobalTranslate((Direction + Weight).Normalized() * (float) delta * Speed);

        }
         
      

    }
}