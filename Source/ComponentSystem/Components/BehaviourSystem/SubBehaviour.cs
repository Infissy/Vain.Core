using System;
using System.Collections.Generic;
using Godot;
using Vain.Core.ComponentSystem.Behaviour;

namespace Vain.Core.ComponentSystem;

public abstract partial class SubBehaviour : BaseNode
{
    protected CharacterBehaviourComponent BehaviourComponent { get; private set; }
    public override void _Ready()
    {
        BehaviourComponent = GetParent<CharacterBehaviourComponent>();
    }

}
