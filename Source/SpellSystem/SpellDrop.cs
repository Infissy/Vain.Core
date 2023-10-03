
using Vain;
using Godot;

//Not workign
using Vain.Core;

namespace Vain.SpellSystem
{
	public partial class SpellDrop : Area2D 
	{
		public SpellChanneler SpellChanneler {get ; set;}
		

		public override void _Ready()
		{
			base._Ready();

			base.Monitoring = true;

			base.BodyEntered += bodyEnteredHandler;

		}
			

		


		void bodyEnteredHandler(Node2D body)
		{
			if(body is Character character)
			{
				var casterComponent = character.GetComponent<SpellCaster>();
				if(casterComponent != null)
				{
					if(casterComponent?.AddSpell(this.SpellChanneler) == true)
						base.QueueFree();

				}

			}
		} 

	}

}
