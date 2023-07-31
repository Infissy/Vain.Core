using Godot;

namespace Vain.Core.ComponentSystem
{
    public partial class MovementControllerComponent : Component
    {
        NavigationAgent3D _agent = default!;

        [Export]
        public float Speed {get; private set;} = 10.0f;
        public float SpeedModifier {get; set;} = 1.0f;
        protected AnimationPlayer? Player {get; private set;}
        

        [Export]
        public Vector3 Target
        {
            get => _agent.TargetPosition;
            set => _agent.TargetPosition = value;
        }

   
        public override void _Ready()
        {
            base._Ready();
            var agent = GetNode<NavigationAgent3D>("../NavigationAgent3D");
            

            if(agent == null)
                //TODO:: Error

            _agent = agent!;

            Player = GetNodeOrNull<Node3D>("Mesh")?.GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
            _agent.TargetPosition = Character.GlobalPosition;   
          
            
        }


        public override void _PhysicsProcess(double delta)
        {   
            base._PhysicsProcess(delta);



            var nextLoc = _agent.GetNextPathPosition();
            var relVec =  nextLoc - Character.GlobalPosition;
            
           
            if(!_agent.IsTargetReached())
            {   
                Character.Velocity = relVec.Normalized() * Speed;

                Character.MoveAndSlide();
                
                nextLoc.Y = Character.GlobalPosition.Y;

                Character.LookAt(nextLoc,Vector3.Up);
                
                if(Player != null && !Player.IsPlaying())
                {
                    Player.Play("Run");
                }

                
            }
                
            
                
        }
        
    }
}