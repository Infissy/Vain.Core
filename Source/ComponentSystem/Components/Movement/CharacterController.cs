using Godot;

namespace Vain
{
    public partial class CharacterController : Component
    {
   
           
        [Export]
        public float Speed {get; private set;} = 10.0f;
        public float SpeedModifier {get; set;} = 1.0f;
        protected NavigationAgent3D Agent {get;private set;}
        protected AnimationPlayer Player {get; private set;}
        
        public override void _Ready()
        {
            base._Ready();
            Agent = GetNode<NavigationAgent3D>("../NavigationAgent3D");
            
            Player = GetNodeOrNull<Node3D>("Mesh")?.GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        }


        public override void _PhysicsProcess(double delta)
        {   
            
            
            base._PhysicsProcess(delta);



            var nextLoc = Agent.GetNextLocation();
            var relVec =  nextLoc - Character.GlobalPosition;

           
            if(!Agent.IsTargetReached())
            {   
                Character.Velocity = relVec.Normalized() * Speed;

                Character.MoveAndSlide();
                
                if(Player != null && !Player.IsPlaying())
                {
                    Player.Play("Run");
                }

                
            }
                
            
                
        }
        
    }
}