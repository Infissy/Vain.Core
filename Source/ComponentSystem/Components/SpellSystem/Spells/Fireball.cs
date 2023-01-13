using Godot;
using System.Collections.Generic;
using Vain;


//Not working



namespace Vain.SpellSystem

{

	public partial class Fireball : ProjectileSpell
	{

		
		


	   
		

		public override void _Process(double delta)
		{   

			base._Process(delta);
		 
			if(LifeTime > 0){

				LifeTime -= (float)delta;
				
			}else{

				QueueFree();

			}
		}

		public override bool Perform(Character owner, Vector3 target)
		{
		  

			var ownerPosition = owner.GlobalPosition;
			GlobalTranslate(ownerPosition);

			Direction = (target-ownerPosition).Normalized();
	 
			


			return true;


		}

		

		public void OnCollision(Node collider)
		{
			
			if(collider is Character character)
				applySpell(character);
		}
		

		
		void applySpell(Character entity)
		{   


				
			
				Effectable effectable = entity.GetComponent<Effectable>(true);
				if( effectable != null)
				{	

					//TODO: FIX  Temp until they enable list export
					var l_effects = new List<Effect>();
					foreach(var effect in this.effects)
					{
						l_effects.Add(effect);
					}
					
				
					effectable.ApplyEffects(l_effects);
					
					NextSpell?.CastSpell(base.Caster,this.GlobalPosition);


					QueueFree();
				
				}
			
		
		}
	}
}
