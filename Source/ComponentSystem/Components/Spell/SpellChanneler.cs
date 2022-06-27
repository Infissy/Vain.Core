using Godot;
namespace Vain.SpellSystem
{

    public class SpellChanneler : Resource
    {
        [Export]
        public Spell Spell;
        
        [Export(PropertyHint.File,"*.tscn")]
        string _spellPrefab;
        [Export(PropertyHint.File,"*.tscn")]
        string _spellDropPrefab;

        [Export]
        public int NumberOfCasts;

        
        public Node SpellPrefab=> GD.Load<PackedScene>(_spellPrefab).Instance();
        public Node SpellDropPrefab => GD.Load<PackedScene>(_spellDropPrefab).Instance();
        

    }

}