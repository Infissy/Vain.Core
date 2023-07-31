using System;
using Godot;
namespace Vain.SpellSystem
{

    public partial class Effect : Resource
    {
        [Export]
        public EffectType effectType;
        [Export]
        public float Duration;
        [Export]
        public float Value;
    }


}