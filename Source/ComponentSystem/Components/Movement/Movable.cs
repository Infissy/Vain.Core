using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


//TODO: Character controller terrain aware 


namespace Vain
{
    

  


    
    public class Movable : Component
    {

        
  
        [Export]
        float _speed = 500;
        [Export]
        float _speedModifier = 1;
        


        
        KinematicBody _collider;

        [Export]
        Vector3 _target;
    
        
        public float Speed => _speed;

        public float SpeedModifier {

            get => _speedModifier;
            set => _speedModifier = value;

        }

        

        public event EventHandler<CollisionEventArgs> OnCollision;




        


        public Vector3 Target {
            get => _target;
            set  => _target = value;
        }

        public Vector3 Position{
            get => _collider.GlobalTransform.origin;
        }


        public override void _Ready()
        {
            
            
            base._Ready();
            _collider = GetChild<KinematicBody>(0) ?? throw new NullReferenceException("No collider found as a child of this component.");
            
          
            
        }




        
        public override void _PhysicsProcess(float delta)
        {   


            if(_target != default)
            {
                KinematicCollision collision = null;

                

                //_collider.LookAt(_target, Vector3.Up);
                
                var movVec = (_target - _collider.GlobalTransform.origin) * _speed * _speedModifier * delta;


                if((_target - _collider.GlobalTransform.origin).Length()<0.1)
                {
                        
                    if(!_collider.TestMove(_collider.Transform,movVec))
                        collision =  _collider.MoveAndCollide(movVec * 0.001f);

                    

                }
                else
                    collision = _collider.MoveAndCollide(movVec); 
                
    
            }
            /*
            if(collision != null)
            {
                if(collision.Collider is Node collider)
                {

                    OnCollision.Invoke(collider);
                }
                if(collision.Collider is Entity colliderEntity)
                
            }
            */
   


        }

        


       
    }
}