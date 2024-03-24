using Vain.HubSystem.Query;

namespace Vain.HubSystem.GameEvent;


public interface IGameEvent<A> where A : IGameEventArgs{}



public interface IGameEventTracked<A> : IGameEvent<A> where A : IGameEventTrackedArgs {}



public interface IGameEventArgs{}
public interface IGameEventTrackedArgs : IGameEventArgs 
{
    uint EventID { get; set; }
}






public interface IListener {}
public interface IListener<E,A> : IListener
    where E : IGameEvent<A>
    where A : IGameEventArgs
{
    void HandleEvent(A args);
}


public interface IListenerTracked<E,A> : IListener<E,A>
    where E : IGameEventTracked<A>
    where A : IGameEventTrackedArgs
{
    void HandleEventTracked(A args);
}



