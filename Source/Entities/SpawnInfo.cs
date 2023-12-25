using Godot;
using Godot.Collections;


using Vain.SpellSystem;


namespace Vain.EnemySystem;

/// <summary>
/// Resource that handles the directives for the <see cref="EnemySpawner"> for spawing a certain kind of enemy. 
/// </summary>
public partial class SpawnInfo : Resource
{

	[Export]
	public PackedScene EnemyPrefab;

	[Export(PropertyHint.ArrayType)]
	public Array<SpellChanneler> EnemySpells;

	[Export]
	public uint NumberOfEnemies;

	//TODO: Export with clamp
	[Export]
	public float ChanceToCastSpell;

}

