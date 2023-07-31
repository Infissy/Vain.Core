using System;
using Godot;
using System.Collections.Generic;
using System.Reflection;

namespace Vain.Singleton
{
    /// <summary>
    /// Manages singletons in the whole game.
    /// Currently it can't delete singletons, and the only way to add one is through SingletonManager.Register(this) in _EnterTree of the script needed;
    /// Look into better ways, eventually with attributes.
    /// </summary>
    partial class SingletonManager : Node
    {

        
        
        static Dictionary<Type,Node> _dictionary = new Dictionary<Type, Node>();
        static List<Node> _persistentNodes = new List<Node>();



    

        public static T? GetSingleton<T>() where T : Node
        {   

            Node singleton;
            bool found =  _dictionary.TryGetValue(typeof(T),out singleton!);


            return found ? singleton as T : null;
        }
        


        public static void Register(Node singleton) 
        {
            if(!_dictionary.ContainsKey(singleton.GetType()))
                _dictionary.Add(singleton.GetType(),singleton);
            else
                _dictionary[singleton.GetType()] = singleton;
        }
        
     
        
        public static void Destroy<T>()
        {
            _dictionary.Remove(typeof(T));
        }
        
    }

  
}