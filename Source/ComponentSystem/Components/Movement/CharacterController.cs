using Godot;

namespace Vain
{
    public partial class CharacterController : Component
    {
        NavigationAgent3D _agent;

        [Export]
        public float Speed {get; private set;} = 10.0f;
        public float SpeedModifier {get; set;} = 1.0f;
        protected AnimationPlayer Player {get; private set;}
        

   
        [Signal]
        public delegate void MovementInputEventHandler(Vector3 target);
        
        public override void _Ready()
        {
            base._Ready();
            _agent = GetNode<NavigationAgent3D>("../NavigationAgent3D");
            
            Player = GetNodeOrNull<Node3D>("Mesh")?.GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
            _agent.TargetPosition = Character.GlobalPosition;   
            MovementInput += (target) => _agent.TargetPosition = target;
           
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