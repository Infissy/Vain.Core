using Godot;
using System.Collections.Generic;



namespace Vain.SpellSystem.UI
{

    /// <summary>
    /// Static class that handles the resources connected to spells.
    /// </summary>
    static class SpellResourceLoader
    {


        static readonly Dictionary<Spell,Texture2D> _sprites = new();


        /// <summary>
        /// Loads the specific sprite in memory.
        /// </summary>
        /// <param name="spell">Spell type</param>
        /// <returns></returns>

        //TODO: Temporary, create a proper system for resource loading
        public static Texture2D LoadSprite(Spell spell)
        {

            return _sprites[spell];

        }


    }
}