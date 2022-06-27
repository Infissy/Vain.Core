
using Vain;
using Godot;

namespace Vain.SpellSystem
{
    public class SpellDrop : Component , IPickable
    {
        public SpellChanneler SpellChanneler {get ; set;}



            

        //temporary, let the user load the spells on its own
        public void  Pickup(Entity entity)
        {
           
                
                entity.ComponentContainer.GetComponent<SpellCaster>().AddSpell(SpellChanneler);
                
                ComponentEntity.Kill();

                
                

            

        }

    }

}
