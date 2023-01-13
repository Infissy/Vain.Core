using System;

using System.Collections.Generic;

using Godot;
using Godot.Collections;



//TODO: Reformat and delete useless stuff


namespace Vain
{

	public partial class EnemySpawner : Node
	{
		



		[Export(PropertyHint.ArrayType)]
		Array<SpawnInfo> _enemies = new Array<SpawnInfo>();
		


		
		Array<SpawnInfo> _tempEnemies = new Array<SpawnInfo>();




		int _diedEnemies;


		Character _player;

		// Start is called before the first frame update
		public override void _Ready()
		{ 

			
			_player = GetNode<PlayableGame>("/root/Game/PlayableGame").PlayableCharacter;


		 
			

			List<Vector3> enemiesLocations = new List<Vector3>();


			if(_enemies != null){
				_diedEnemies =  _enemies[0].NumberOfEnemies;
			}


			
		}

		public override void _Process(double delta){
			if(_diedEnemies > 0 && _player != null ){
				for (int i = 0; i < _diedEnemies; i++)
				{ 

					var instance = _enemies[0].NPCPrefab.Instantiate<NPC>();

					Vector3 deltapos = (new Vector3(GD.Randf(), 0, GD.Randf()).Normalized() * 30);
					
					
					AddChild(instance);
					instance.GlobalPosition = _player.GlobalPosition + deltapos;
					
					//TODO: Fix resource instantiation, even duplicate can't avoid making the resource shared between enemies, at the moment every enemy has their own spells preassigned
					/*
					NPCSpellCaster caster = instance.GetComponent<NPCSpellCaster>();
					caster.AddSpells(_enemies[0].EnemySpells);
					caster.ChanceToCast = _enemies[0].ChanceToCastSpell;
					*/

					instance.CharacterKilled += EnemyDestroyed;
				}

				_diedEnemies = 0;
			}
		
		}

		public void EnemyDestroyed(){
			GD.Print("Enemy Destroyed");
			_diedEnemies++;

		}

	

	
		
	
	}

}
