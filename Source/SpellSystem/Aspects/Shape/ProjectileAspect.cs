
namespace Vain.SpellSystem.Aspects;
internal class ProjectileAspect : Aspect
{
    public override string Name => "projectile_aspect";


    public override void Initialize(ref SpellInitializationSpecification initSpecs, ref SpellUpdateSpecification updateSpecs)
    {
        base.Initialize(ref initSpecs, ref updateSpecs);
        
        initSpecs.SpawnOnTarget = false;
        updateSpecs.direction = (initSpecs.Target - initSpecs.Caster).Normalized();
        updateSpecs.Velocity =  100f;
        
    }
}