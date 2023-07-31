using Godot;
using Vain.Core;
using Vain.Singleton;

namespace Vain.SpellSystem.UI
{
    /// <summary>
    /// UI Element that shows player spells.
    /// </summary>
    
  
    public partial class SpellBar : HBoxContainer
    {
        [Export]
        PackedScene _spellSlot;


        Character _currentCharacter;
        public override void _EnterTree()
        {
            SingletonManager.Register(this);
        }

        public override void _Ready()
        {
            base._Ready();

            var player = SingletonManager.GetSingleton<Player>();
            
            
            _currentCharacter = SingletonManager.GetSingleton<Player>().CurrentCharacter;

            _currentCharacter.GetComponent<SpellCaster>().SpellPickup += loadSpells;
            
            player.CurrentCharacterChanged +=  (oldCharacter) =>
            {
                oldCharacter.GetComponent<SpellCaster>().SpellPickup -= loadSpells;
                _currentCharacter.GetComponent<SpellCaster>().SpellPickup += loadSpells;
            };
            

            loadSpells();
        }

        //TODO: Full reload, maybe optimize it 
        void loadSpells()
        {
            foreach (var child in GetChildren())
            {
                RemoveChild(child);
                child.QueueFree();
            }

            var channelers = _currentCharacter.GetComponent<SpellCaster>().SpellChannelers;
            foreach (SpellChanneler channeler in channelers)
            {
                
                if (channeler != null)
                {
                    var spellSlot = _spellSlot.Instantiate<SpellSlot>();



                    AddChild(spellSlot);
                    
                    spellSlot.Channeler = channeler;

                }


            }


        }

    } 
    
}