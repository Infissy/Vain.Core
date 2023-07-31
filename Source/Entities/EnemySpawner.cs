using System;
using System.Linq;
using System.Collections.Generic;

using Vain.Singleton;


using Godot;

//TODO: Reformat and delete useless stuff


using Vain.SpellSystem;
using Vain.Core;

namespace Vain.EnemySystem
{

	/// <summary>
	/// EnemySpawner handles the spawn of hostile NPCs in combat scenarios.
	/// </summary>

	public partial class EnemySpawner : Node 
	{
		





		Dictionary<string,uint> _enemiesToSpawn = new Dictionary<string, uint>();
		uint _diedEnemies;
		Character? _player;




		[Export]
		public bool Active {get;set;}

		[Export]
		Godot.Collections.Dictionary<string,SpawnInfo> SpawnInfo {get;set;} = new Godot.Collections.Dictionary<string, SpawnInfo>();


		public override void _EnterTree()
		{
			base._EnterTree();
			SingletonManager.Register(this);
		}



		public override void _Ready()
		{ 
			
			
			_player = SingletonManager.GetSingleton<Player>().CurrentCharacter;
			
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
			if(_diedEnemies > 0 && _player != null && Active ){


			
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

		/// <param name="enemyName"></param>
		/// <param name="count"></param>
		public void SpawnEnemy(String enemyName, uint count)
		{

			if(!SpawnInfo.ContainsKey(enemyName))
				return;
			
			
			
			
			for (int i = 0; i < count; i++)
			{ 
				
				var instance = SpawnInfo[enemyName].EnemyPrefab.Instantiate<Character>();
					

				//instance.GetComponent<NPCController>().Behaviour = SpawnInfo[enemyName].Behaviour;
				
	

				Vector3 deltapos = (new Vector3(GD.Randf(), 0, GD.Randf()).Normalized() * 30);
				
				
				AddChild(instance);
				instance.GlobalPosition = _player!.GlobalPosition + deltapos;
				
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

}
