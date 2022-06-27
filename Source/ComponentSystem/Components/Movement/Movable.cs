using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vain
{
    

  


    
    public class Movable : Component
    {

        
  
        [Export]
        float _speed = 500;
        [Export]
        float _speedBoost = 1;
    


    

        public float Speed => _speed;


        public float SpeedBoost;

        Vector2 _target = Vector2.Zero;



        public delegate void CollisionHandler(Node collider);

        public event CollisionHandler OnCollision;



        public Vector2 Target {
            get => _target;
            set  => _target = value;
        }

        public override void _Ready()
        {
            
            
            base._Ready();

            
            
        }


        public override void _PhysicsProcess(float delta)
        {   
            
            Vector2 direction = (_target - ComponentEntity.Position);
            
            KinematicCollision2D collision = null;
            if(direction.Length()<10)
            {
                    
                if(!ComponentEntity.TestMove(ComponentEntity.Transform,direction.Normalized() * _speed * _speedBoost))
                  collision =  ComponentEntity.MoveAndCollide(direction.Normalized() * _speed * _speedBoost * 0.001f);

                

            }
            else
                collision = ComponentEntity.MoveAndCollide(direction.Normalized() * _speed * _speedBoost); 

                
            if(collision != null)
            {
                if(collision.Collider is Node collider)
                {

                    OnCollision.Invoke(collider);
                }
                if(collision.Collider is Entity colliderEntity)
                
            }
            
   


        }

        


       
    }
}