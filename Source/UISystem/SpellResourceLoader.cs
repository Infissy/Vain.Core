using Godot;
using System.Collections.Generic;



namespace Vain.SpellSystem.UI
{
    static class SpellResourceLoader
    {
        

        static Dictionary<Spell,Texture2D> _sprites = new Dictionary<Spell, Texture2D>();
        
        //TODO: Temporary, create a proper system for resource loading


        
        public static Texture2D LoadSprite(Spell spell)
        {
        
            if(!_sprites.ContainsKey(spell))
            {
                switch (spell)
                {
                    case Spell.NONE:
                        _sprites.Add(spell,null);
                        break;
                    case Spell.FIREBALL:
                        _sprites.Add(spell, ResourceLoader.Load<Texture2D>("res://Resources/2D/Spell/Fireball.png"));
                        break;
                }


            }   

            return _sprites[spell];

        }


    }
}