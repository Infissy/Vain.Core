using Vain;

using Godot;
//Not working


namespace Vain.SpellSystem
{
	public partial class PlayerSpellCaster : SpellCaster 
	{

		MainCamera _camera;
		public override void _Ready()
		{
			base._Ready();
			_camera = SingletonManager.GetSingleton<MainCamera>();
		}

		


		public override void _UnhandledInput(InputEvent @event)
		{
			
			if (@event is InputEventKey keyEvent && keyEvent.Pressed)
			{       
				switch (keyEvent.Keycode)
				{
					case Key.Q:
					CastSpell(0,_camera.GetMouseScenePosition());
						break;
				}
			}

		}
	}
}
