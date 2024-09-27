using Godot;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using static Vain.HubSystem.GameEvent.GameEvents.Spell;
using static Vain.HubSystem.Query.Queries;

namespace Vain.SpellSystem.UI;





public partial class SpellBarUI : Control,
    IListener<PlayerSpellInputEvent, PlayerSpellInputEventArgs>
{



    int _currentIndex = 0;
    bool _casting;

    public override void _Ready()
    {

        Hub.Instance.Subscribe(this);

        AddChild(new HBoxContainer());


    }

    public void HandleEvent<E>(PlayerSpellInputEventArgs args)
    {
        if (args.Input == SpellInput.EnterCast)
        {
            FetchPath();
            _casting = true;
            return;
        }
        else if (args.Input == SpellInput.ExitCast)
        {
            _casting = false;

            foreach (var child in GetChild(0).GetChildren())
                child.QueueFree();
            _currentIndex = 0;
            return;
        }

        var element = GetChild(0).GetChild(_currentIndex + (int)args.Input) as SpellInputElement;
        element.Pressed = true;
        _currentIndex *= 4;
    }



    void FetchPath()
    {
        var response = Hub.Instance.QueryData<SpellPathQuery, EmptyQueryRequest, SpellPathQueryResponse>(new EmptyQueryRequest());

        if (response == null)
            return;

        int direction = 0;

        foreach (var spell in response?.FirstLayer)
        {
            var spellInput = new SpellInputElement((SpellInputElement.SpellInputType)(direction % 4));
            GetChild(0).AddChild(spellInput);
            direction++;
        }
    }

}
