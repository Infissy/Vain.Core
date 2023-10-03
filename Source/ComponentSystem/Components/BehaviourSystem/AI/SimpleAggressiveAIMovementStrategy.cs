using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;

namespace Vain.Core.ComponentSystem.Behaviour
{
	enum NPCAIBehaviour
	{
		FollowPlayer,
		KeepDistance

	}
    public partial class SimpleAggressiveAIMovementStrategy : AIMovementStrategy
    {

		
     

        [Export]
		public float DistanceToPlayer {get; set;}
		[Export]
		NPCAIBehaviour AIBehaviourType {get;set;}
		

        public override Vector2 BehaviourTick(Character character,AIController controller)
        {	
			
			if(controller.AggressionLevel < controller.HostileCharacter.Position.DistanceTo(character.GlobalPosition) )
				return character.GlobalPosition;



            Vector2 targetPosition = character.GlobalPosition; 
		


			//TODO: Add Jitter or random movements 
			switch(AIBehaviourType){


				case NPCAIBehaviour.FollowPlayer:

					return controller.HostileCharacter.GlobalPosition;




				case NPCAIBehaviour.KeepDistance:
					
		
					
					
					return targetPosition =  controller.HostileCharacter.GlobalPosition - (controller.HostileCharacter.GlobalPosition - character.GlobalPosition).Normalized() * DistanceToPlayer ;

			

				}

			return character.GlobalPosition;
        }



    }
}