using Godot;
namespace Vain.Core
{

		

	public partial class Player : Character
	{
		
		
		[Signal]
		public delegate void PlayerDeathEventHandler();
	
		public override void _EnterTree()
		{
			base._EnterTree();
			SingletonManager.Register(this);

			
		}


		public override void Kill()
		{
			
			EmitSignal(SignalName.PlayerDeath);


			
					
			
			base.Kill();
		
		}

	}

}
