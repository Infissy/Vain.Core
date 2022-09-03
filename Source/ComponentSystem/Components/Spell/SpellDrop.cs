
using Vain;
using Godot;

//Not workign


namespace Vain.SpellSystem
{
    public class SpellDrop : Component , IPickable
    {
        public SpellChanneler SpellChanneler {get ; set;}



            

        //temporary, let the user load the spells on its own
        public void  Pickup(Entity entity)
        {
           
                
                GetComponent<SpellCaster>().AddSpell(SpellChanneler);
                
                Entity.Kill();

                
                

            

        }

    }

}
