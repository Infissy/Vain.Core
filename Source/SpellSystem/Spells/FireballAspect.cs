using Vain.SpellSystem.Aspects;

class FireballAspect : Aspect
{
   
    public override  string Name => "FireballAspect";



    public override Aspect[] Aspects => new Aspect[]{
        new ProjectileAspect(),
        new FireAspect(),
    };
}