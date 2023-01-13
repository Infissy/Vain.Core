using Godot;
using System.Linq;
using Vain.Console;

namespace Vain
{

	   public enum NPCAIBehaviour{
		FollowPlayer,
		KeepDistance,

	}
	
	
	public partial class NPCController : CharacterController , HostileTargetBehavior
	{

		[Export]        
		NPCAIBehaviour enemyAIType = NPCAIBehaviour.KeepDistance;
		
		public Character HostileTarget {get;set;}
		

		[Export]
		public float DistanceToPlayer {get; set;}



		[Export]
		public float AvoidanceDistance{get; set;}

		Area3D _avoidanceArea;

		[ExportGroup("Debug")]
		
		
		[Export]
		public bool Debug_ShowTarget{ get; set; }
		CSGSphere3D _debugMesh;


		public override void _Ready()
		{
			base._Ready();




			if(Debug_ShowTarget)
			{ 
				_debugMesh = new CSGSphere3D();
				_debugMesh.Radius = 0.2f;
				_debugMesh.Material = new StandardMaterial3D() { AlbedoColor = Colors.Red};

			}
			
			
			
			AddChild(_debugMesh);

            _avoidanceArea = GetNode<Area3D>("Area3D");

            var collider = _avoidanceArea.GetChild<CollisionShape3D>(0).Shape as SphereShape3D;
            collider.Radius = AvoidanceDistance;

        }





        public override void _Process(double delta)
		{
		 
			base._Process(delta);

			if(HostileTarget != null)
				AgentTick();
			
			
		}
	

		void AgentTick()
		{
		


			Vector3 targetPosition = Character.GlobalPosition; 


			//TODO: Add Jitter or random movements 
			switch(enemyAIType){
				case NPCAIBehaviour.FollowPlayer:
					targetPosition = HostileTarget.GlobalPosition;


					break;



				case NPCAIBehaviour.KeepDistance:
					
		
					
					
					targetPosition =    HostileTarget.GlobalPosition - (HostileTarget.GlobalPosition - Character.GlobalPosition).Normalized() * DistanceToPlayer ;

					break;

				}


            var insideBodies = _avoidanceArea.GetOverlappingBodies().OfType<NPC>().Where(npc => npc != Character);

    

            if (insideBodies.Count() > 0)
            {
                var relativePositions = insideBodies.Select<Node3D, Vector3>(body => body.GlobalPosition - Character.GlobalPosition);


                Vector3 sum = Vector3.Zero;
                foreach (var position in relativePositions)
                {
                    sum += position;
                }

                var relEnemyPosition = sum / insideBodies.Count();

                var averageEnemyPosition = Character.GlobalPosition + relEnemyPosition;

                //Opposite of averageEnemyPosition, best direction(not normalized) to get away from enemies, 
                var avoidPosition = (Character.GlobalPosition - averageEnemyPosition).Normalized() * AvoidanceDistance + Character.GlobalPosition;



                //Takes the average position of the enemies, and weights on depending on the distance to the npc
                //Nearer the average position is to the NPC more the NPC weights the direction to get away from enemies
                targetPosition = ((avoidPosition - targetPosition) * (1 - ((averageEnemyPosition - Character.GlobalPosition).Length() / AvoidanceDistance))) + targetPosition;


            }




			if(Debug_ShowTarget)
				_debugMesh.GlobalPosition = targetPosition;

            Agent.TargetLocation = targetPosition;
			//TODO: add human like delay to movements to simulate reaction time. 


		}


	}   

 
}
