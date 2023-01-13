using Godot;

namespace Vain
{
    public partial class CharacterController : Component
    {
   
           
        [Export]
        public float Speed {get; private set;} = 10.0f;
        public float SpeedModifier {get; set;} = 1.0f;
        protected NavigationAgent3D Agent {get;private set;}

        
        public override void _Ready()
        {
            base._Ready();
            Agent = GetNode<NavigationAgent3D>("../NavigationAgent3D");

            
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
                

                
            }
                
            
                
        }
        
    }
}