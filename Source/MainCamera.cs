using Godot;

using Vain.Singleton;
using Vain.Log;



namespace Vain.Core;


/// <summary>
/// Main Game Camera
/// </summary>

public partial class MainCamera : Camera2D , IEntity
{


    Vector2 _oldPlayerPosition;

    /// <summary>
    /// Character to follow.
    /// </summary>

    public Singleton<Character> Player {get;set;}

    public uint RuntimeID {get; private set;}


    public override void _EnterTree()
    {
        base._EnterTree();
        SingletonManager.Register(SingletonManager.Singletons.MAIN_CAMERA,this);
        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference?.Register(this);
    }


    public override void _Ready()
    {
        base._Ready();


        Player = SingletonManager.GetSingleton<Character>(SingletonManager.Singletons.PLAYER, ()=> this.GlobalPosition = Player.Reference.GlobalPosition);


        if(Player.Reference != null)
            _oldPlayerPosition = Player!.Reference!.GlobalPosition;

    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(Player.Reference == null)
            return;

        var relMotion =  Player!.Reference!.GlobalPosition - _oldPlayerPosition;


        this.GlobalTranslate(relMotion);

        _oldPlayerPosition = Player.Reference.GlobalPosition;

    }


    public Vector2 GetMouseScenePosition()
    {
        return ((GetTree().Root.GetMousePosition() - (GetTree().Root.Size / 2)) / 4) + this.GlobalPosition;
    }

    public override void _ExitTree()
    {
        base._EnterTree();
        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Free(this);

    }

}
