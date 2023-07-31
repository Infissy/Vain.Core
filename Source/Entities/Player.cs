using Godot;

using Vain.Singleton;


namespace Vain.Core
{
	//Works as a wrapper for the current controlled
	public partial class Player : Node
	{
		Character _currentCharacter;
		public Character CurrentCharacter 
		{	
			get
			{
				return _currentCharacter; 
			}
			internal set
			{
				if(_currentCharacter != null)
				{
					CurrentCharacter.CharacterKilled -= OnDeath;
				}

				var oldCharacter = _currentCharacter;
				_currentCharacter = value;
				_currentCharacter.CharacterKilled += OnDeath;

				EmitSignal(SignalName.CurrentCharacterChanged,oldCharacter);


			}
			
		}

		[Signal]
		public delegate void CurrentCharacterChangedEventHandler(Character oldCharacter);
		[Signal]
		public delegate void PlayerDeathEventHandler();
	
		public override void _EnterTree()
		{
			base._EnterTree();
			SingletonManager.Register(this);
			
			
		}

		public void OnDeath()
		{
			EmitSignal(SignalName.PlayerDeath);
		}

	}
	

}
