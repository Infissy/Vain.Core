using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace Vain.SpellSystem.Aspects;


class FireAspect : Aspect
{
    public override string Name => "aspect_fire";

    public void Hit(ref SpellHitSpecification spellSpecs, ref SpellUpdateSpecification spellUpdateSpecs)
    {
    }

    public  void Initialize ( ref SpellInitializationSpecification initSpecs, ref SpellUpdateSpecification updateSpecs) 
    {
        updateSpecs.Color = updateSpecs.Color.Blend(Colors.OrangeRed);
        updateSpecs.Velocity /= 2f;
        updateSpecs.Size -= 0.3f;
    }

    public void Process(ref SpellUpdateSpecification spellUpdateSpecs, double delta)
    {
    }
}