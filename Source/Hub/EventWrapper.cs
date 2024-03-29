using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Vain.HubSystem.Query;


namespace Vain.HubSystem.GameEvent;
abstract internal class EventWrapper
{
    public Type Event {get;set;}
    public  IGameEventArgs EventArgs { get; set; }
    protected EventWrapper() {}
    protected EventWrapper(IGameEventArgs args, Type type) 
    {
        EventArgs = args;
        Event = type;
    }
    public abstract void DispatchToListeners(List<IListener> listeners);
}

internal class TypedEventWrapper<E,A> : EventWrapper
    where E : IGameEvent<A>
    where A : IGameEventArgs
{
    public TypedEventWrapper(IGameEventArgs args) : base(args,typeof(E)) {}
    public override void DispatchToListeners(List<IListener> listeners)
    {
        foreach(var listener in listeners)
        {
            (listener as IListener<E,A>).HandleEvent<E>((A)EventArgs);
        }
    }
}


internal class TypedEventWrapperTracked<E,A> : EventWrapper
    where E : IGameEventTracked<A>
    where A : IGameEventTrackedArgs
    {
        public TypedEventWrapperTracked(IGameEventArgs args) : base(args,typeof(E)) {}
        

        public override void DispatchToListeners(List<IListener> listeners)
        {
            foreach(var listener in new List<IListener>(listeners))
            {
                if (listener is IListenerTracked<E, A> provider)
                {
                    provider.HandleEventTracked<E>((A)EventArgs);   
                }
                else
                {
                    (listener as IListener<E, A>).HandleEvent<E>((A)EventArgs);
                }
            }

        }
    }