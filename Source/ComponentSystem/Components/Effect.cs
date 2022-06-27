using System;
namespace Vain{



    public enum EffectType{
        STUN,
        PULL,
        HEAL,
        BURN,
        SPEEDBOOST,
        DAMAGE,

    }

    
    public struct Effect{

        public EffectType effectType;
        public float Duration;
        public float Value;
    }


}