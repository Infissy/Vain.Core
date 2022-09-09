using Godot;
using System.Collections.Generic;
using Vain;


//Not working



namespace Vain.SpellSystem

{

    public class Fireball : SpellBehaviour, IInitialize, IPhysicsProcessable
    {

        
        

        [Export] 
        bool _multipleTicks;




        [Export]

        float _speed;

        Vector3 _direction;

        bool _castByPlayer;
        float _totalLifetime;



        Movable _movable;


        public void Initialize(){
            

        
       
            
            _movable = GetComponent<Movable>();



            

            _movable.SpeedModifier += this._speed - _movable.Speed;
            
            _movable.OnCollision += collisionHandler;

            

            _totalLifetime = Lifetime ;

        
        }

        public void PhysicsProcess(float delta)
        {   


            _movable.Target =  _movable.Position + _direction;
            
            

        }

        public void Process(float delta)
        {   

         
            if(Lifetime > 0){

                Lifetime -= delta;
                
            }else{

                Entity.Kill();

            }
        }

        public override bool Cast(Entity owner, Vector3 target)
        {

            if(GetComponent<Player>() != null)
                _castByPlayer = true;
            else
               _castByPlayer = false;




            var ownerPosition = owner.GetComponent<Movable>().Position;
            _movable.ForcePosition(ownerPosition);

            _direction = (target-ownerPosition).Normalized();
     
            


            return true;


        }

        

        void collisionHandler(object sender, CollisionEventArgs collider)
        {

            if(collider.Collider is Entity entity)
                if(entity.GetComponent<Player>(true) != null ^ _castByPlayer)
                    applySpell(entity);
        }
        

        
        void applySpell(Entity entity)
        {   


                
            
                Hittable hittable = entity.GetComponent<Hittable>(true);
                Effectable effectable = entity.GetComponent<Effectable>(true);
                if(hittable != null && effectable != null)
                {
                
                    effectable.ApplyEffects(this.effects);
        
                    Entity.Kill();
                
                }
            
        
        }
    }
}