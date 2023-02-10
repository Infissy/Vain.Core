using System;
using Godot;
using System.Collections.Generic;
namespace Vain
{
    //TODO: At the moment using empty interfaces, find a more elegant way to handle singletons (Custom Attribute ? )

    class SingletonManager
    {

        static Dictionary<Type,Node> _dictionary = new Dictionary<Type, Node>();
        public static T GetSingleton<T>() where T : Node
        {
            return (T) _dictionary[typeof(T)];
        }
        
        public static T GetSigletonOrDefault<T>() where T : Node
        {
            try
            {
                return GetSingleton<T>();
            }
            catch (KeyNotFoundException)
            {
                
                return default(T);
            }            
        }


        public static void Register(Node singleton) 
        {
            _dictionary.Add(singleton.GetType(),singleton as Node);
        }

        

        
        public static void Destroy<T>()
        {
            _dictionary.Remove(typeof(T));
        }
    }




  
}