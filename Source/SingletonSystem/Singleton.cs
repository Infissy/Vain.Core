using Godot;
using System;
using System.Collections.Generic;

namespace Vain.Singleton
{
    /// <summary>
    /// Proxy handle for singletons in the game.
    /// It's advised to always use the Reference property to get the singleton value at a certain moment.
    /// Please do not bind events to the refence, use RegisterToSignal for proper event handling. 
    /// It's possible to get the signal name through Class.SignalName.Event
    /// </summary>
 
    public class Singleton <T> : SingletonHandle where  T : Node
    {


        bool _disposed = false;
        T _reference;


        Dictionary<string,List<Callable>> _boundCallbacks = new Dictionary<string, List<Callable>>();



        public T Reference 
        {   
            get => _reference;
            internal set
            {

                updateReference(value);
                _reference = value;
            
            
            }
            
        }
        public override bool Disposed 
        {
            get => _disposed;
            internal set
            {
                _disposed = value;
                Reference = null;
            }
            
        }
        
        public void RegisterToSignal(string signal, Callable callback)
        {
            if(!_boundCallbacks.ContainsKey(signal))
                _boundCallbacks.Add(signal,new List<Callable>());

            _boundCallbacks[signal].Add(callback);
        
        }

        public Singleton(T instance)
        {
            Reference = instance;
        }
        

        protected virtual void updateReference(T newReference)
        {
            foreach (var signal in _boundCallbacks)
            {

                foreach (var callback in signal.Value)
                {
                    _reference.Disconnect(signal.Key,callback);
                    
                }


            }


            foreach (var signal in _boundCallbacks)
            {

                foreach (var callback in signal.Value)
                {
                    _reference.Connect(signal.Key,callback);
                    
                }


            }
        }


    }
    
    


    public abstract class SingletonHandle
    {
        public virtual bool Disposed {get; internal set;} = false;
    }

}