using Godot;
using System.Linq;
using Vain.Console;

namespace Vain
{

	   public enum NPCAIBehaviour{
		FollowPlayer,
		KeepDistance,

	}
	
	
	public partial class NPCController : CharacterController 
	{

		
	

		[Export] 
		public NPCBehaviour Behaviour {get; set;}


		[Export]
		public float AvoidanceDistance{get; set;}

		Area3D _avoidanceArea;

		[ExportGroup("Debug")]
		[Export]
		public bool Debug_ShowTarget{ get; set;}
		CsgSphere3D _debugMesh;


		public override void _Ready()
		{
			base._Ready();




			if(Debug_ShowTarget)
			{ 
				_debugMesh = new CsgSphere3D();
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

		
			AgentTick();
			
			
		}
	
	
		void AgentTick()
		{
			

			var targetPosition = Behaviour.BehaviourTick(base.Character as NPC);
			


			targetPosition = avoidFilter(targetPosition);


			//TODO: add human like delay to movements to simulate reaction time. 


			if(Debug_ShowTarget)
				_debugMesh.GlobalPosition = targetPosition;

            EmitSignal(SignalName.MovementInput,targetPosition);
		


		}

		//TODO: Maybe look into moving the filter to the external behaviour to be adaptive to the situation
		Vector3 avoidFilter(Vector3 targetLocation)
		{
			
			

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
                targetLocation = ((avoidPosition - targetLocation) * (1 - ((averageEnemyPosition - Character.GlobalPosition).Length() / AvoidanceDistance))) + targetLocation;


            }

			return targetLocation;

		}


	}   

 
}
