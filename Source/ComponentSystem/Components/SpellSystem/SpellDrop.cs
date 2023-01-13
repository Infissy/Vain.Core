
using Vain;
using Godot;

//Not workign


namespace Vain.SpellSystem
{
	public partial class SpellDrop : Area3D 
	{
		public SpellChanneler SpellChanneler {get ; set;}
		

		public override void _Ready()
		{
			base._Ready();

			base.Monitoring = true;

			base.BodyEntered += bodyEnteredHandler;
		}
			

		


		void bodyEnteredHandler(Node3D body)
		{
			if(body is Character character)
			{
				var casterComponent = character.GetComponent<SpellCaster>(true);
				if(casterComponent != null)
				{
					if(casterComponent?.AddSpell(this.SpellChanneler) == true)
						base.QueueFree();

				}

			}
		} 

	}

}
