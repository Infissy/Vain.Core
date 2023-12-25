using Godot;
using Vain.Core;
using Vain.Singleton;

namespace Vain.LevelSystem;

[GlobalClass]
public partial class SpawnPoint : Node2D , IEntity
{
    uint _runtimeID;
    public uint RuntimeID => _runtimeID;

    [Export]
    public string Tag {get;set;}


    public override void _Ready()
    {
        base._Ready();
        _runtimeID = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Register(this);
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Free(this);
    }
}
