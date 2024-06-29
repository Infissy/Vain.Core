using System;

namespace Vain.SpellSystem.Aspects;





class InstancedDamangeAspect : Aspect
{
    public float Damage = 0.0f;
    public override string Name => "Single Damage";


    public InstancedDamangeAspect(float damage) 
    {
        Damage = damage;
    }

}