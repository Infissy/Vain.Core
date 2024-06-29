using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using Vain.Singleton;
using static Vain.HubSystem.GameEvent.GameEvents;

namespace Vain.UI;
public partial class PlayerHealthBar : HealthBar,
    IListener<PlayerHealthUpdate,CharacterHealthUpdateArgs>
{
    public override void _Ready()
    {
        base._Ready();
        Hub.Instance.Subscribe(this);

    }

    public void HandleEvent<PlayerHealthUpdate>(CharacterHealthUpdateArgs args)
    {
        Health = args.Health;
    }


    public override void _ExitTree(){
        Hub.Instance.Unsubscribe(this);
    }
}
