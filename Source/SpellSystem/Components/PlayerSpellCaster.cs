using Vain;

using Godot;
using Vain.Core;

using Vain.UI;

namespace Vain.SpellSystem
{

	
	public partial class PlayerSpellCaster : SpellCaster 
	{

		private class ACTIONS
		{
			public const string SLOT1 = "player_cast_slot1";
			public const string SLOT2 = "player_cast_slot2";
			public const string SLOT3 = "player_cast_slot3";
			public const string SLOT4 = "player_cast_slot4";
			public const string SLOT5 = "player_cast_slot5";
			public const string SLOT6 = "player_cast_slot6";
			public const string SLOT7 = "player_cast_slot7";
			public const string SLOT8 = "player_cast_slot8";
		}
	
		MainCamera _camera;
		public override void _Ready()
		{
			base._Ready();
			_camera = SingletonManager.GetSingleton<MainCamera>();
			
		}

		


		public override void _UnhandledInput(InputEvent @event)
		{
			//TODO: Refactor, find a better way to handle actions without a long list of ifs
			if (Input.IsActionJustPressed(ACTIONS.SLOT1))
			{       
				base.CastSpell(0,_camera.GetMouseScenePosition());
			}
			if (Input.IsActionJustPressed(ACTIONS.SLOT2))
			{       
				base.CastSpell(1,_camera.GetMouseScenePosition());
			}
			if (Input.IsActionJustPressed(ACTIONS.SLOT3))
			{       
				base.CastSpell(2,_camera.GetMouseScenePosition());
			}

			if (Input.IsActionJustPressed(ACTIONS.SLOT4))
			{       
				base.CastSpell(3,_camera.GetMouseScenePosition());
			}
			if (Input.IsActionJustPressed(ACTIONS.SLOT5))
			{       
				base.CastSpell(4,_camera.GetMouseScenePosition());
			}

			if (Input.IsActionJustPressed(ACTIONS.SLOT6))
			{       
				base.CastSpell(5,_camera.GetMouseScenePosition());
			}
			if (Input.IsActionJustPressed(ACTIONS.SLOT7))
			{       
				base.CastSpell(6,_camera.GetMouseScenePosition());
			}
			if (Input.IsActionJustPressed(ACTIONS.SLOT8))
			{       
				base.CastSpell(7,_camera.GetMouseScenePosition());
			}
		}
	}
}
