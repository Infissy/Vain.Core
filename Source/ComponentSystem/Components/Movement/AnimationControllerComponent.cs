using System;
using Godot;
using Vain.SpellSystem;

namespace Vain.Core.ComponentSystem
{  
    
    public partial class AnimationControllerComponent : Component
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