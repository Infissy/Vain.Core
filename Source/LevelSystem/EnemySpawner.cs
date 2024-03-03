using System;
using System.Linq;
using System.Collections.Generic;

using Vain.Singleton;


using Godot;

//TODO: Reformat and delete useless stuff


using Vain.SpellSystem;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.HubSystem;


using System.Diagnostics;
using static Vain.HubSystem.Query.Queries;

namespace Vain.EnemySystem;


/// <summary>
/// EnemySpawner handles the spawn of hostile NPCs in combat scenarios.
/// </summary>

public partial class EnemySpawner : Node
{

	Dictionary<string,uint> _enemiesToSpawn = new();
	uint _diedEnemies;



	[Export]
	public bool Active {get;set;}

	[Export]
	Godot.Collections.Dictionary<string,SpawnInfo> SpawnInfo {get;set;} = new Godot.Collections.Dictionary<string, SpawnInfo>();


	public override void _Ready()
	{


		if(SpawnInfo != null)
		{
			foreach (var spawnInfo in SpawnInfo)
			{

				_enemiesToSpawn.Add(spawnInfo.Key,0);

			}
		}



		//TODO: Spawn enemies from external system(game rule)
		SpawnRandomEnemy(10);
	}

	public override void _Process(double delta){
		//TODO: Spawn for enemy type
		if(_diedEnemies > 0 && Active ){

			SpawnRandomEnemy(_diedEnemies);

			_diedEnemies = 0;
		}

	}



	public void SpawnRandomEnemy(uint count)
	{


		var names = SpawnInfo.Keys;
		var randN = GD.RandRange(0,names.Count-1);
		GD.Print(names);
		SpawnEnemy(names.ElementAt(randN),count);


	}

	public void SpawnEnemy(string enemyName, uint count)
	{

		if(!SpawnInfo.ContainsKey(enemyName))
			return;

		var result = Hub.Instance.QueryData<PlayerPositionQuery,EmptyQueryRequest,PositionQueryResponse>();

		if(result == null)
			return;

		var playerPosition = result?.Position ?? new Vector2();


		for (int i = 0; i < count; i++)
		{
			var instance = SpawnInfo[enemyName].EnemyPrefab.Instantiate<Character>();


			//instance.GetComponent<NPCController>().Behaviour = SpawnInfo[enemyName].Behaviour;



			Vector2 deltapos = new Vector2(GD.Randf(), GD.Randf()).Normalized() * 30;

			AddChild(instance);

			instance.GlobalPosition =  playerPosition + deltapos;

			//TODO: Fix resource instantiation, even duplicate can't avoid making the resource shared between enemies, at the moment every enemy has their own spells preassigned
			//TODO: Create a system to check if a system is enabled as to add components
			var caster = instance.GetComponent<SpellCaster>();
			caster?.AddSpells(SpawnInfo[enemyName].EnemySpells);


			//caster.ChanceToCast = SpawnInfo[enemyName].ChanceToCastSpell;


			instance.CharacterKilled += EnemyDestroyed;
		}


	}



	public void EnemyDestroyed(){

		_diedEnemies++;

	}

}
