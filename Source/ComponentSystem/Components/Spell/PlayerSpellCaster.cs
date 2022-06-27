
using Godot;

namespace Vain.SpellSystem
{
    public class PlayerSpellCaster : SpellCaster 
    {

        
        


        public override void _Input(Godot.InputEvent @event)
        {
            base._Input(@event);
            var mousepos =  MainCamera.Instance.Position;
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {       
                switch ((KeyList)keyEvent.Scancode)
                {
                    case KeyList.Q:
                    CastSpell(0,mousepos);
                        break;
                }
            }

        }
    }
}
