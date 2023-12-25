using Godot;

namespace Vain.SpellSystem.UI
{

    /// <summary>
    /// UI Slot that contains sprite and number of casts left.
    /// </summary>
    partial class SpellSlot : Control
    {

        int _castCount;


        //TODO: Take a look in the future, I'm not sure atm is it's the most elegant way to handle the connections
        public SpellChanneler Channeler 
        {
            set
            {
                CastCount = value.CastCount;
                Spell = value.Spell;                

                var set_count = () => {
                    
                    this.CastCount = value.CastCount;
                };
                
                value.Changed += set_count;

                this.TreeExiting += ()=>
                {
                    value.Changed -= set_count;
                };

            }
        }


        int CastCount 
        {
         
            set
            {
                
                _castCount = value - 1;
                if(_castCount > 0)
                {
                    GetNode<Label>("Count").Text = _castCount.ToString(); 
                }
                else
                {
                    GetNode<Label>("Count").Text = "";
                    //Spell = Spell.NONE;
                }
                
                 

            }

        }


        Spell Spell
        {
            set
            {
           
                GetNode<TextureRect>("Sprite").Texture = SpellResourceLoader.LoadSprite(value);
                
            }
        }
    
    
    
        
    }




}