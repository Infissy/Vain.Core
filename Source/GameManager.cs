using Godot;
namespace Vain.Core;

/// <summary>
/// Highest parent of the game SceneTree, stores and handles data between scenes (Maps/Menus).
/// </summary>
public partial class GameManager : Node
{
    [Signal]
    public delegate void SceneChangeEventHandler();

}
