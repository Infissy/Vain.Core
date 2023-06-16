using System;
using Godot;
using Vain.SpellSystem;
namespace Vain.Core
{  
    
    public partial class MeshController : Component
    {
        enum Animation 
        {
            RUN,
            IDLE,
            LAUNCH,
            CAST
        }
        AnimationPlayer _animationPlayer;
        public override void _Ready()
        {
            base._Ready();

            _animationPlayer = GetChild<AnimationPlayer>(1);


            GetComponent<SpellCaster>().SpellCast += (_) => 
            {
                
                _animationPlayer.Play("Cast");
                _animationPlayer.Advance(0);
            };
            GetComponent<CharacterController>().MovementInput += (_)=>
            {
                _animationPlayer.Play("Run");
                _animationPlayer.Advance(0);
            };

        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            
           
            
            if(Character.Velocity.Length() == 0)
            {
                if(_animationPlayer.CurrentAnimation != "Cast")
                {
                    _animationPlayer.Play("Idle");

                }
                
            }
            
            

        }
           

        
        
    } 
}