using Godot;

namespace  Vain.SpellSystem.Aspects;



class AreaAspect : Aspect
{

    public float Radius { get; private set; } = 5f;
    public Color Color { get; private set; } = Colors.Red;
    public override string Name => "Area Spell";

    public override string[] Templates => new string[]{
        "Giant Area",
        "Small Area",
    };



    public AreaAspect(string template) {
        SelectedTemplate = template;
    }

    public override void Process(ref SpellUpdateSpecification spellUpdateSpecs, double delta)
    {

        spellUpdateSpecs.Size = Radius;
        spellUpdateSpecs.Color = Color;
    }


}