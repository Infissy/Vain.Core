using Godot;

using Vain.SpellSystem;

namespace Vain.Core.ComponentSystem.Behaviour
{
    partial class PlayerSpellCasterBehaviour : SubBehaviour
    {
        

        public const string SLOT1 = "player_cast_slot1";
        public const string SLOT2 = "player_cast_slot2";
        public const string SLOT3 = "player_cast_slot3";
        public const string SLOT4 = "player_cast_slot4";
        public const string SLOT5 = "player_cast_slot5";
        public const string SLOT6 = "player_cast_slot6";
        public const string SLOT7 = "player_cast_slot7";
        public const string SLOT8 = "player_cast_slot8";


        public override void _UnhandledInput(InputEvent input)
        {


            if(input is not InputEventAction action)
                return;


            if(!action.Action.ToString().Contains("player_cast_slot"))
                return;

        
            var spellCaster = BehaviourCluster.Character.GetComponent<SpellCaster>();
            

            switch (action.Action)
            {
               
            }
        
    }

    }
}