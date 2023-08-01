using Godot;
using System.Linq;
using Vain.Console;

namespace Vain.Core.ComponentSystem.Behaviour
{

	  
	
	
	public partial class AIController : SubBehaviour 
	{

		

		CsgSphere3D _debugMesh;
		
		
		//TODO:: Handle a table of aggros in case it's needed
		[Export]
		internal float AggressionLevel {get;set;}
		
		[Export]
		AIMovementStrategy AI {get;set;}

		[Export]
		internal float AvoidanceDistance{get; set;}


		[Export]
		Area3D _avoidanceArea;

		[ExportGroup("Debug")]
		[Export]
		public bool Debug_ShowTarget{ get; set;}
		



		internal Character HostileCharacter{get;set;}


		
		
		
		public override void _Ready()
		{

            _avoidanceArea = GetNode<Area3D>("Area3D");

            var collider = _avoidanceArea.GetChild<CollisionShape3D>(0).Shape as SphereShape3D;
            collider.Radius = AvoidanceDistance;

        }





        public override void _Process(double delta)
		{
		 
			var targetPosition = AI.BehaviourTick(BehaviourCluster.Character,this);
			


			targetPosition = avoidFilter(targetPosition);


			//TODO: add human like delay to movements to simulate reaction time. 


			if(Debug_ShowTarget)
				_debugMesh.GlobalPosition = targetPosition;

            
		

		}
	
		//TODO: Maybe look into moving the filter to the external behaviour to be adaptive to the situation
		Vector3 avoidFilter(Vector3 targetLocation)
		{
			
			var character = BehaviourCluster.Character;

            var insideBodies = _avoidanceArea.GetOverlappingBodies().Where(npc => npc != character);

    

            if (insideBodies.Count() > 0)
            {
                var relativePositions = insideBodies.Select<Node3D, Vector3>(body => body.GlobalPosition - character.GlobalPosition);


                Vector3 sum = Vector3.Zero;
                foreach (var position in relativePositions)
                {
                    sum += position;
                }

                var relEnemyPosition = sum / insideBodies.Count();

                var averageEnemyPosition = character.GlobalPosition + relEnemyPosition;

                //Opposite of averageEnemyPosition, best direction(not normalized) to get away from enemies, 
                var avoidPosition = (character.GlobalPosition - averageEnemyPosition).Normalized() * AvoidanceDistance + character.GlobalPosition;



                //Takes the average position of the enemies, and weights on depending on the distance to the npc
                //Nearer the average position is to the NPC more the NPC weights the direction to get away from enemies
                targetLocation = ((avoidPosition - targetLocation) * (1 - ((averageEnemyPosition - character.GlobalPosition).Length() / AvoidanceDistance))) + targetLocation;


            }

			return targetLocation;

		}


	}   

 
}
