using Godot;

namespace Vain
{
    public partial class SimpleAggressiveBehaviour : NPCBehaviour
    {

        [Export]        
		NPCAIBehaviour enemyAIType {get;set;}


        [Export]
		public float DistanceToPlayer {get; set;}



        public override Vector3 BehaviourTick(NPC character)
        {
            Vector3 targetPosition = character.GlobalPosition; 


			//TODO: Add Jitter or random movements 
			switch(enemyAIType){
				case NPCAIBehaviour.FollowPlayer:
					targetPosition = character.HostileTarget.GlobalPosition;


					break;



				case NPCAIBehaviour.KeepDistance:
					
		
					
					
					targetPosition =    character.HostileTarget.GlobalPosition - (character.HostileTarget.GlobalPosition - character.GlobalPosition).Normalized() * DistanceToPlayer ;

					break;

				}

            return targetPosition;
        }



    }
}