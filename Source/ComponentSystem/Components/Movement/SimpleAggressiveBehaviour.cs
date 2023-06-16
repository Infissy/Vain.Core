using Godot;
using Vain.Core;

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

			if(character.AggressionLevel < character.HostileTarget.Position.DistanceTo(character.GlobalPosition) )
				return character.GlobalPosition;



            Vector3 targetPosition = character.GlobalPosition; 
		


			//TODO: Add Jitter or random movements 
			switch(enemyAIType){


				case NPCAIBehaviour.FollowPlayer:

					return character.HostileTarget.GlobalPosition;




				case NPCAIBehaviour.KeepDistance:
					
		
					
					
					return targetPosition =    character.HostileTarget.GlobalPosition - (character.HostileTarget.GlobalPosition - character.GlobalPosition).Normalized() * DistanceToPlayer ;

			

				}

			return character.GlobalPosition;
        }



    }
}