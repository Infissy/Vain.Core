using Godot;
namespace Vain
{
	
	[Singleton]
	public partial class Player : Character
	{
		
		

	

		public override void Kill()
		{
			

			this.GetTree().Root.GetTree().ReloadCurrentScene();
			
			
			base.Kill();
		
		}

	}

}
