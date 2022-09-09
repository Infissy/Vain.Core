using Godot;

using Vain.Godot;
namespace Vain
{

    [Tool]
    class InteractibleEntity : Entity
    {
        [Export(PropertyHint.PlaceholderText,"Test,,Test2")]
        EffectType[] components;

        public override void _Ready()
        {
            base._Ready();

            
        }
    }
}