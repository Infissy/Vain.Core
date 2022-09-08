
using System;
using System.Collections.Generic;
using Godot;
using Vain.Log;


// Effectable component for a entity to be able to get an effect from something

namespace Vain
{
    
    class Effectable : Component, IInitialize
    {


        

        List<Effect> effects = new List<Effect>();
        
        List<Effect> effectsApplied = new List<Effect>();


       
        Movable _movable;


        Health _health;

        

        public void Initialize()
        {
            
            _movable = GetComponent<Movable>();
        }
        
        public void ApplyEffects(List<Effect> effectsToApply)
        {
        
            foreach (Effect effect in effectsToApply)
            {
                for (int i = 0; i < effectsToApply.Count; i++)
                {
                    if(effects.Contains(effectsToApply[i])){

                        switch (effectsToApply[i].effectType)
                        {
                            //TODO: Add different handling for different effects (resets cooldown or applies more damage)

                            
                        }

                    }else{
                        
                        effects.Add(effect);

                    }


                }
            }   

        }


        public void IProcessable(float delta)
        {   
            if(effects.Count > 0 || effectsApplied.Count > 0)
                handleEffects(delta);
        
        }

        void handleEffects(float delta){

            for (int i = 0; i < effects.Count; i++)
            {
                

                switch(effects[i].effectType){
                    case EffectType.DAMAGE:
                        Logger.SetContext(Entity).Debug("Entity Damaged");
                        _health?.Damage(effects[i].Value);
                        
                        break;
                    case EffectType.SPEEDBOOST:
                        if(_movable != null)
                        {
                            _movable.SpeedModifier = effects[i].Value;

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
                        
                        if (_movable != null)
                            _movable.SpeedModifier = 1-effectsApplied[i].Value;

                            
                        break;


                    }
                    effectsApplied.RemoveAt(i);
                }else{


                    Effect eff = effectsApplied[i];
                    
                    eff.Duration -= delta;
                    
                    Logger.SetContext(Entity).Debug(eff.Duration.ToString());
                    effectsApplied[i] = eff; 

                }



                

            }
            
        }

    
    }
}