
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Godot;


namespace Vain.SpellSystem.Aspects;

enum HitType {
    Character,
    Spell,
    StaticBody,
}

internal abstract class Aspect
{
   
    public abstract string Name { get; } 
    public virtual string[] Templates { get; } = new string[] {"Default"};

    public virtual Aspect[] Aspects { get; } = Array.Empty<Aspect>();



    public string SelectedTemplate {get; protected set;} = "Default";
    public virtual void Initialize (ref SpellInitializationSpecification spellInitializationSpecs,ref SpellUpdateSpecification spellUpdateSpecs) {
        foreach(var aspect in Aspects)
            aspect.Initialize(ref spellInitializationSpecs,ref spellUpdateSpecs);
    }

    public virtual void Process(ref SpellUpdateSpecification spellUpdateSpecs, double delta) {
        foreach(var aspect in Aspects)
            aspect.Process(ref spellUpdateSpecs, delta);
    }

    public virtual void Hit (ref SpellHitSpecification  spellSpecs, ref SpellUpdateSpecification spellUpdateSpecs) {

        foreach(var aspect in Aspects)
            aspect.Hit(ref spellSpecs,ref spellUpdateSpecs);
    }




    public static string GetName<T>() where T : Aspect => Activator.CreateInstance<T>().Name;
    public static string[] GetTemplates<T>() where T : Aspect => Activator.CreateInstance<T>().Templates;
    
}

struct SpellFlags {
    public bool ReflectSpell { get; set; }
    
}


internal struct SpellInitializationSpecification {
    public Vector2 Target;
    public Vector2 Caster;
    public bool SpawnOnTarget = true;
    public SpellInitializationSpecification(Vector2 caster, Vector2 target)
    {
        Caster = caster;
        Target = target;
    }

}


internal struct SpellHitSpecification {

    public HitType HitType;

    public short SpellPenetrationCount = 0;
    public short PenetrationIndex = 0;
    public bool DestroySelf = true;
    public Vector2 AngleOfReflection = Vector2.Zero;
    public SpellHitSpecification(HitType hitType) => HitType = hitType;
}


internal class SpellUpdateSpecification {


    public Color Color = Colors.White;
    public float Size = 1.0f;
    public Vector2 direction = Vector2.Zero;
    public float Velocity = 0;

    public int Penetration = 1;
    public int PenetrationIndex = 1;
    public SpellUpdateSpecification()
    {
    }
}