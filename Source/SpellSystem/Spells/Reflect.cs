using Godot;
using Vain.Core;

namespace Vain.SpellSystem.Spells
{
    public partial class Reflect : AreaSpell
    {
        [Export]
        public float DistanceToPlayer {get;set;}


        public override void _Ready()
        {
            base._Ready();
            base.BodyEntered += OnCollision;
            
        }
        
        internal override bool Perform(Character owner, Vector2 target)
        {
            var direction = owner.GlobalPosition - target;

            var position = direction * (direction.Length() / Mathf.Clamp(DistanceToPlayer,0,direction.Length())) + owner.GlobalPosition;         
            
            this.GlobalPosition = position;


            return true;
            
        }
        
        internal override void Hint(Character owner, Vector2 target)
        {
            
        }
        
        public void OnCollision(Node collider)
        {
            
            if(collider is ProjectileSpell spell)
                spell.Direction = - spell.Direction;
        }
		

    
    }
}