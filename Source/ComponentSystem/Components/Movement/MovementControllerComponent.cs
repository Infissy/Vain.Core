using Godot;
using Vain.Log;

namespace Vain.Core.ComponentSystem;

[GlobalClass]
public partial class MovementControllerComponent : Component
{
    NavigationAgent2D _agent = default!;

    [Export]
    public float Speed {get; private set;} = 40.0f;
    public float SpeedModifier {get; set;} = 1.0f;
    protected AnimationPlayer Player {get; private set;}


    [Export]
    public Vector2 Target
    {
        get => _agent.TargetPosition;
        set => _agent.TargetPosition = value;
    }


    public override void _Ready()
    {
        base._Ready();
        var agent = GetNode<NavigationAgent2D>("../NavigationAgent2D");
        
        #if DEBUG
        agent.DebugEnabled = true;
        #endif

        
        _agent = agent!;
        _agent.PathDesiredDistance = 24;
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
        
            if(Player != null && !Player.IsPlaying())
            {
                Player.Play("Run");
            }

        }
            
        
            
    }
    
}
