
using Godot;

//Not working


namespace Vain.SpellSystem
{
    public class PlayerSpellCaster : SpellCaster , IUnhandledInputHandler
    {

        
        


        public void UnhandledInput(InputEvent @event)
        {
            
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {       
                switch ((KeyList)keyEvent.Scancode)
                {
                    case KeyList.Q:
                    CastSpell(0,MainCamera.Instance.MouseTargetInScene);
                        break;
                }
            }

        }
    }
}
