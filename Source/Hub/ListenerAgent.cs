using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;
namespace Vain.HubSystem.GameEvent;


public class ListenerAgent<E,A> : IListener<E,A>
    where E : IGameEvent<A>
    where A : IGameEventArgs
{
    public TaskCompletionSource<A> Task {get; private set;} = new();
    public ListenerAgent()
    {
        Hub.Instance.Subscribe(this);
    }
    public void HandleEvent(A args)
    {
        Task.SetResult(args);
        Hub.Instance.Unsubscribe(this);
    }
    
    

}


public class ListenerAgentTracked<E,A> : IListener<E,A>
    where E : IGameEventTracked<A>
    where A : IGameEventTrackedArgs
{
    uint _tracking = 0;
    List<A> _calls = new();
    TaskCompletionSource<A> _source = new();
    public Task<A> Result => _source.Task;
    public ListenerAgentTracked()
    {
        Hub.Instance.Subscribe(this);
       
    }

    public void Listen(uint tracking)
    {
        _tracking = tracking;
        foreach (var call in _calls)
        {
            HandleEvent(call);   
        }
    }
    public void HandleEvent(A args)
    {

        if(_tracking == 0)
        {
            _calls.Add(args);
            return;
        }   
            


        if(args.EventID == _tracking)
        {
            _source.SetResult(args);
            Hub.Instance.Unsubscribe(this);
        }
    }

}