
using Godot;

//Not working


namespace Vain.SpellSystem
{
	public partial class PlayerSpellCaster : SpellCaster 
	{

		
		


		public override void _UnhandledInput(InputEvent @event)
		{
			
			if (@event is InputEventKey keyEvent && keyEvent.Pressed)
			{       
				switch (keyEvent.Keycode)
				{
					case Key.Q:
					CastSpell(0,MainCamera.Instance.MouseTargetInScene);
						break;
				}
			}

		}
	}
}
