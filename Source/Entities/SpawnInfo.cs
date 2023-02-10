using Godot;
using Godot.Collections;


using Vain.SpellSystem;


namespace Vain
{
    public partial class SpawnInfo : Resource
    {

			[Export]
			public PackedScene NPCPrefab;
			
			[Export(PropertyHint.ArrayType)]
			public Array<SpellChanneler> EnemySpells;
			
			[Export]
			public int NumberOfEnemies;
			
			//TODO: Export with clamp
			[Export]
			public float ChanceToCastSpell;
			[Export]
			public NPCBehaviour Behaviour;
		}

}