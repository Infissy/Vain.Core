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
        public int NumberOfCasts {get; set;}

        


        public Node InstantiateSpell()
        {
         
            return GD.Load<PackedScene>(_spellPrefab).Instance();

        }

     
        public Node InstantiateSpellDrop()
        {
            return GD.Load<PackedScene>(_spellDropPrefab).Instance();
        }


    }

}