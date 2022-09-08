using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


//TODO: Character controller terrain aware 


namespace Vain
{
    

  


    
    public class Movable : Component, IInitialize, IPhysicsProcessable
    {

        
  
        [EditableField]
        float _speed = 500;
        [EditableField]
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
        public event EventHandler<CollisionEventArgs> OnMovementUpdate;



        


        public Vector3 Target {
            get => _target;
            set  => _target = value;
        }

        public Vector3 Position{
            get => _collider.GlobalTransform.origin;
        }


        public void Initialize()
        {
            
            
            
            //TODO: Kinematic body initalization

            //_collider = OfType<KinematicBody>().FirstOrDefault() ?? throw new NullReferenceException("No collider found as a child of this component.");
        
        }



        public void ForcePosition(Vector3 position)
        {

            //TODO: Debug eventual problems, movement synching missing
            _collider.Translate(position);

            
        }

        
        public void PhysicsProcess(float delta)
        {      
            

            if(_target != default)
            {
                KinematicCollision collision = null;

                

                var movVec = (_target - _collider.GlobalTransform.origin).Normalized() * _speed * _speedModifier * delta;

                
                if((_target - _collider.GlobalTransform.origin).Length() < 0.001)
                {
                        
                    if(!_collider.TestMove(_collider.Transform,movVec))
                        collision =  _collider.MoveAndCollide(movVec * 0.001f);

                    

                }
                else
                    _collider.MoveAndCollide(movVec); 
      
                
                
                if(collision != null)
                {
                    if(collision.Collider is Node collider)
                    {

                        OnCollision.Invoke(this, new CollisionEventArgs(collider));
                    }
                    
                    
                }
                
    
            }
        }

    }
}