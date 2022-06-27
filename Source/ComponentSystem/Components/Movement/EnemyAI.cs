using Godot;
using Vain.Console;

namespace Vain
{

       public enum EnemyAIType{
        FollowPlayer,
        KeepDistance,

    }
    
    
    public class EnemyAI : Component
    {

        
        EnemyAIType enemyAIType = EnemyAIType.KeepDistance;



        
        Movable _movable;

        public override void _Ready()
        {
            base._Ready();

            _movable = GetParent<ComponentContainer>().GetComponent<Movable>();

        }


        Entity _playerEntity;


        [Export]
        public float DistanceToPlayer = 10;
        [Export]
        public float JitterDistance;
        [Export]
        public float StasisDelayThreshold;
        [Export]
        public float MoveDelayThreshold;

        
        float moveDelay;
        float stasisDelay;
        bool statusUpdated = false;
        bool playerHasMoved = true;


        Vector2 jitterVector;
        Vector2 oldPlayerPosition;

        



        public override void _Process(float delta)
        {
            
            base._Process(delta);
            
            // TODO : Check for null has to be done elsewhere
            if(_playerEntity != null){

                


                switch(enemyAIType){
                    case EnemyAIType.FollowPlayer:


                        _movable.Target = _playerEntity.Position ;


                        break;
                    case EnemyAIType.KeepDistance:
                        
                
                        Vector2 posRelative =   _playerEntity.Position - ComponentEntity.Position ; 
                        
                        
                        //TODO: Add Jitter or random movements 

                        Vector2 target =    _playerEntity.Position -  posRelative.Normalized() * DistanceToPlayer ;

                        
                        
                        _movable.Target =  target ;

                        break;

                    }
                

                //TODO: add human like delay to movements to simulate reaction time. 


            }
            else
            {
                _playerEntity = Player.Instance.Entity;
            }

            




        }

        

    }   

 
}