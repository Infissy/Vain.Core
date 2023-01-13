using Godot;
namespace Vain
{
	

	public partial class Player : Character
	{
		
		Camera3D _camera;

		
		

	

		public override void Kill()
		{
			

			this.GetTree().Root.GetTree().ReloadCurrentScene();
			
			
			base.Kill();
		
		}

	}

}
