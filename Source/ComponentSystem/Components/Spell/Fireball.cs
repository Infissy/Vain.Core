using Godot;
using System.Collections.Generic;
using Vain;

namespace Vain.SpellSystem

{

    public class Fireball : SpellBehaviour
    {

        
        

        [Export] 
        bool _multipleTicks;




        [Export]

        float _speed;

        Vector2 _direction;

        bool _castByPlayer;
        float _totalLifetime;



        Movable _movable;


        public override void _Ready(){
            

            base._Ready();
            SetPhysicsProcess(false);
            SetProcess(false);
            
            
            _movable = ComponentEntity.ComponentContainer.GetComponent<Movable>();



            

            _movable.SpeedBoost += this._speed - _movable.Speed;
            
            _movable.OnCollision += collisionHandler;

            

            _totalLifetime = Lifetime ;

        
        }

        public override void _PhysicsProcess(float delta)
        {   

            base._PhysicsProcess(delta);

            _movable.Target =  ComponentEntity.Position + _direction;
            
            

        }

        public override void _Process(float delta)
        {   

            base._Process(delta);

            if(Lifetime > 0){

                Lifetime -= delta;
                
            }else{

                ComponentEntity.Kill();

            }
        }

        public override bool Cast(Entity owner, Vector2 target)
        {

            if(owner.ComponentContainer.GetComponent<Player>() != null)
                _castByPlayer = true;
            else
                _castByPlayer = false;




            var ownerPosition = owner.Position;
            ComponentEntity.Position = ownerPosition;

            _direction = (target-ownerPosition).Normalized();
            this.SetPhysicsProcess(true);
            this.SetProcess(true);
            
            


            return true;


        }

        

        void collisionHandler(Node collider)
        {

            if(collider is Entity entity)
                if(entity.ComponentContainer.GetComponent<Player>(true) != null ^ _castByPlayer)
                    applySpell(entity);
        }
        

        
        void applySpell(Entity entity)
        {   


                
            
                Hittable hittable = entity.ComponentContainer.GetComponent<Hittable>(true);
                Effectable effectable = entity.ComponentContainer.GetComponent<Effectable>(true);
                if(hittable != null && effectable != null){
                
                    effectable.ApplyEffects(this.effects);
        
                    ComponentEntity.Kill();
                
                }
            
        
        }
    }
}