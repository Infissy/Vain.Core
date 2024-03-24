using System;
using System.Collections.Generic;

using Vain.HubSystem.Query;
using Vain.HubSystem.GameEvent;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Godot;
using Vain.Log;



namespace Vain.HubSystem;


public class Hub
{
    static public Hub Instance => _instance ??= new Hub();
    static Hub _instance;


    readonly ConcurrentQueue<EventWrapper> _eventQueue = new();
    readonly Dictionary<Type,List<IListener>> _listeners = new();
    //TODO: Add multiple data providers with priority (so if one can't provide there is a fallback)
    readonly Dictionary<Type,IDataProvider> _dataProviders = new();


    readonly List<uint> _trackings = new();


    public void Subscribe<E,A>(IListener<E,A> listener)
        where E : IGameEvent<A>
        where A : IGameEventArgs
    {

        RuntimeInternalLogger.Instance.Debug($"Subscribing {listener} to {typeof(E)}");

        if(!_listeners.ContainsKey(typeof(E)))
            _listeners[typeof(E)] = new List<IListener>();

        _listeners[typeof(E)].Add(listener);
    }


    public void Unsubscribe<E,A>(IListener<E,A> listener)
        where E : IGameEvent<A>
        where A : IGameEventArgs
    {
        RuntimeInternalLogger.Instance.Debug($"Unsubscribing {listener} from {typeof(E)}");
        if(_listeners.ContainsKey(typeof(E)))
            _listeners[typeof(E)].Remove(listener);

    }

    public void Emit<E,A>(A gameEvent)
        where E : IGameEvent<A>
        where A : IGameEventArgs
    {
        RuntimeInternalLogger.Instance.Debug($"Emitting {typeof(E)}");

        _eventQueue.Enqueue(new TypedEventWrapper<E,A>(gameEvent));
    }

    public uint EmitTracked<E,A>(A gameEvent)
        where E : IGameEventTracked<A>
        where A : IGameEventTrackedArgs
    {

        RuntimeInternalLogger.Instance.Debug($"Emitting tracked {typeof(E)}");
        if(gameEvent.EventID == 0)
        {
            uint tracking;
            do {
                tracking = GD.Randi();
            }
            while(_trackings.Contains(tracking));

            gameEvent.EventID = tracking;
        }
        //TODO: Remove tracking id when not needeed anymore
        
        _eventQueue.Enqueue(new TypedEventWrapperTracked<E,A>(gameEvent));

        return gameEvent.EventID;
    }



    public void RegisterDataProvider<Q,Rq,Rs>(IDataProvider<Q, Rq, Rs> dataProvider)
        where Q : IQuery<Rq,Rs>
        where Rq : IRequest
        where Rs : struct, IQueryResponse
    {
        RuntimeInternalLogger.Instance.Debug($"Registering data provider {dataProvider} for {typeof(Q)}");
        
        _dataProviders[typeof(Q)] = dataProvider;
    }

    public void UnregisterDataProvider<Q,Rq,Rs>(IDataProvider<Q, Rq, Rs> dataProvider)
        where Q : IQuery<Rq,Rs>
        where Rq : IRequest
        where Rs : struct, IQueryResponse
    {
        RuntimeInternalLogger.Instance.Debug($"Unregistering data provider {dataProvider} for {typeof(Q)}");


        _dataProviders.Remove(typeof(Q));
    }

    public Rs? QueryData<Q,Rq,Rs>(Rq request = default)
        where Q : IQuery<Rq,Rs>
        where Rq : IRequest
        where Rs : struct, IQueryResponse
    {
        //RuntimeInternalLogger.Instance.Debug($"Querying data provider for {typeof(Q)}");


        if(!_dataProviders.ContainsKey(typeof(Q)))
            return null;


        return ((IDataProvider<Q, Rq, Rs>)_dataProviders[typeof(Q)]).Provide(request);


    }


    internal void ConsumeEvents()
    {

        while (_eventQueue.TryDequeue(out var wrapper))
        {
            RuntimeInternalLogger.Instance.Debug($"Dispatching event {wrapper.Event} with { (_listeners.ContainsKey(wrapper.Event) ? _listeners[wrapper.Event].Count : 0 ) } listeners");
            
            if(!_listeners.ContainsKey(wrapper.Event))
                continue;

            wrapper.DispatchToListeners(_listeners[wrapper.Event]);

        }
    }


}


