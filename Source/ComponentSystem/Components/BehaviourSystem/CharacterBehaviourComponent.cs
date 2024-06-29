using Godot;
using Godot.Collections;

namespace Vain.Core.ComponentSystem;

/// <summary>
/// Special component, contains the behaviours for the various components
/// </summary>
[GlobalClass]
public partial class CharacterBehaviourComponent : Component
{
    public void AddSubBehaviour(SubBehaviour subBehaviour)
    {
        //TODO: FIX Temporary fix for multithreading
        CallDeferred(MethodName.AddChild, subBehaviour);
    }
}
