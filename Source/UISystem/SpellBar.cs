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


        Singleton<Character> _currentCharacter;
        public override void _EnterTree()
        {
            SingletonManager.Register(SingletonManager.Singletons.UI.SPELL_BAR,this);
        }

        public override void _Ready()
        {
            base._Ready();

           
            
            _currentCharacter = SingletonManager.GetSingleton<Character>(SingletonManager.Singletons.PLAYER);

            var caster = _currentCharacter?.Reference?.GetComponent<SpellCaster>();
            if(caster != null)
                caster.SpellPickup += loadSpells;
            
            

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

            var channelers = _currentCharacter.Reference.GetComponent<SpellCaster>().SpellChannelers;
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