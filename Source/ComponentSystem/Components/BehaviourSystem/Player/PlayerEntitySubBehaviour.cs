using Godot;
using Vain.HubSystem;
using Vain.HubSystem.Query;
using Vain.Singleton;
using static Vain.HubSystem.Query.Queries;

namespace Vain.Core.ComponentSystem;

[GlobalClass]
/// <summary>
/// Required to register the current character as the player
/// </summary>
public partial class PlayerEntitySubBehaviour : SubBehaviour,
        IDataProvider<PlayerPositionQuery,EmptyQueryRequest,PositionQueryResponse>
{
    public override void _Ready()
    {
        base._Ready();
        Hub.Instance.RegisterDataProvider(this);
    }


    public PositionQueryResponse? Provide(EmptyQueryRequest request)
    {
        return new PositionQueryResponse{Position = BehaviourComponent.Character.GlobalPosition};
    }
    public override void _ExitTree()
    {
        base._EnterTree();
        Hub.Instance.UnregisterDataProvider(this);
    }
}


