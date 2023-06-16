using Godot;
using Vain.Core;

namespace Vain.SpellSystem.UI
{
    /// <summary>
    /// UI Element that shows player spells.
    /// </summary>
    
  
    public partial class SpellBar : HBoxContainer
    {
        [Export]
        PackedScene _spellSlot;


        Character _player;
        public override void _EnterTree()
        {
            SingletonManager.Register(this);
        }

        public override void _Ready()
        {
            base._Ready();


            _player = SingletonManager.GetSingleton<Player>();

            _player.GetComponent<SpellCaster>().SpellPickup += loadSpells;


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

            var channelers = _player.GetComponent<SpellCaster>().SpellChannelers;
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