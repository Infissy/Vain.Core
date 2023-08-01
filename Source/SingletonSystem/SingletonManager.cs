using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


using Godot;


namespace Vain.Singleton
{
    /// <summary>
    /// Manages singletons in the whole game.
    /// Currently it can't delete singletons, and the only way to add one is through SingletonManager.Register(this) in _EnterTree of the script needed;
    /// Look into better ways, eventually with attributes.
    /// </summary>
    public partial class SingletonManager : Node
    {

        
        
        static Dictionary<string,Node> _dictionary = new Dictionary<string, Node>();

        static Dictionary<string,Singleton<Node>> _

        public static Singleton<T>? GetSingleton<T>(string key) where T : Node
        {   

            Node singleton;
            bool found =  _dictionary.TryGetValue(key,out singleton!);
            if(!found || singleton is not T instance)
                return null;


            return new Singleton<T>(instance);
        }
        


        public static void Register(string key,Node singleton) 
        {
            if(!_dictionary.ContainsKey(key))
                _dictionary.Add(key,singleton);
            else
                _dictionary[key] = singleton;
        }
        

        public static IEnumerable<string> GetSingletonsList()
        {
            return _dictionary.Keys;
        }


        public static void Destroy(string key)
        {
            _dictionary.Remove(key);
        }




        public static class Singletons
        {

            public static class UI
            {
                public static string SPELL_BAR = "ui_spell_bar";
                public static string HEALTH_BAR = "ui_health_bar";
            }
            public static string MAIN_CAMERA = "main_camera";
            public static string LEVEL_MANAGER = "level_manager";
            public static string PLAYER = "entity_player";
            
        }
        
    }

  
}