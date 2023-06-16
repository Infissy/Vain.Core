
using System;
using System.Collections.Generic;
using Vain.Log;

using Vain.SpellSystem;
// Effectable component for a entity to be able to get an effect from a spell


//TODO: Refactor 
namespace Vain
{
	
	partial class Effectable : Component
	{


		

		List<Effect> effects = new List<Effect>();
		
		List<Effect> effectsApplied = new List<Effect>();


	   	[RequiredComponent]
		CharacterController _characterController;

		[RequiredComponent]
		Health _health;

		

		public override void _Ready()
		{
			base._Ready();
			_characterController = GetComponent<CharacterController>(true);
			_health = GetComponent<Health>(true);
		}
		
		public void ApplyEffects(List<Effect> effectsToApply)
		{
			effects.AddRange(effectsToApply);


		}


		public override void _Process(double delta)
		{   
			if(effects.Count > 0 || effectsApplied.Count > 0)
				handleEffects((float)delta);
		
		}

		void handleEffects(float delta){

			for (int i = 0; i < effects.Count; i++)
			{
				

				switch(effects[i].effectType){
					case EffectType.DAMAGE:
						//Logger.SetContext(Character).Debug("Entity Damaged");
						_health?.Damage(effects[i].Value);
						
						break;
					case EffectType.SPEEDBOOST:
						if(_characterController != null)
						{
							_characterController.SpeedModifier = effects[i].Value;

						}
						
						break;                
				}


				
				Effect eff = effects[i];
				eff.Duration -= delta;
				effects[i] = eff; 
				
				effectsApplied.Add(effects[i]);
				effects.RemoveAt(i);
				
			}

			for (int i = 0; i < effectsApplied.Count; i++)
			{
				
				if(effectsApplied[i].Duration <= 0)
				{
					switch (effectsApplied[i].effectType)
					{

						case EffectType.SPEEDBOOST:
						
						if (_characterController != null)
							_characterController.SpeedModifier = 1-effectsApplied[i].Value;

							
						break;


					}
					effectsApplied.RemoveAt(i);
				}else{


					Effect eff = effectsApplied[i];
					
					eff.Duration -= delta;
					
					Logger.SetContext(Character).Debug(eff.Duration.ToString());
					effectsApplied[i] = eff; 

				}



				

			}
			
		}

	
	}
}
