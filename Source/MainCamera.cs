using Godot;

using Vain.Singleton;
using Vain.HubSystem.Query;

using static Vain.HubSystem.Query.Queries;
using Vain.HubSystem;


namespace Vain.Core;


/// <summary>
/// Main Game Camera
/// </summary>

public partial class MainCamera : Camera2D , IEntity,
    IDataProvider<MousePositionQuery,EmptyQueryRequest,PositionQueryResponse>
{


    Vector2 _oldPlayerPosition;

    /// <summary>
    /// Character to follow.
    /// </summary>



    public uint RuntimeID {get; set;}


    public override void _EnterTree()
    {
        base._EnterTree();
        Hub.Instance.RegisterDataProvider(this);

    }


    public override void _Ready()
    {
        base._Ready();




        var queryResult = Hub.Instance.QueryData<PlayerPositionQuery,EmptyQueryRequest,PositionQueryResponse>();
        if(queryResult != null)
            _oldPlayerPosition = queryResult?.Position ?? Vector2.Zero;

    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        var queryResult = Hub.Instance.QueryData<PlayerPositionQuery,EmptyQueryRequest,PositionQueryResponse>();
        if(queryResult == null)
            return;

        var position =  queryResult?.Position ?? Vector2.Zero;
        var relMotion = position - _oldPlayerPosition;


        this.GlobalTranslate(relMotion);

        _oldPlayerPosition = position;

    }




    public Vector2 GetMouseScenePosition()
    {
        return ((GetTree().Root.GetMousePosition() - (GetTree().Root.Size / 2)) / 4) + this.GlobalPosition;
    }

    public override void _ExitTree()
    {
        base._EnterTree();
        Hub.Instance.UnregisterDataProvider(this);
        

    }



    public PositionQueryResponse? Provide(EmptyQueryRequest request)
    {
        return new PositionQueryResponse {Position = GetMouseScenePosition() };
    }
}


