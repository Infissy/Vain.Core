using Godot;
using System;
using Vain.Configuration;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using static Vain.HubSystem.GameEvent.GameEvents.Spell;
using static Vain.HubSystem.Query.Queries;

namespace Vain.SpellSystem.UI;





public partial class SpellBarUI : Node,
    IListener<PlayerSpellInputEvent, PlayerSpellInputEventArgs>
{


    PackedScene _spellInput;
    int _currentIndex = 0;
    bool _casting;
    
    public override void _Ready()
    {
        var path = ProjectConfiguration.LoadConfiguration("UI","SpellInputElement")[0];
        _spellInput = ResourceLoader.Load<PackedScene>(path);
        
    
    }

    public void HandleEvent<E>(PlayerSpellInputEventArgs args)
    {
        if(args.Input == SpellInput.EnterCast)
        {
            FetchPath();
            _casting = true;
        }
        if(args.Input == SpellInput.ExitCast)
            _casting = false;

        

        var script = (GDScript) GetChild(0).GetChild(_currentIndex).GetScript();
        script.Set("pressed",true);
        
    }



    void FetchPath() 
    {
        var response = Hub.Instance.QueryData<SpellPathQuery,EmptyQueryRequest,SpellPathQueryResponse>(new EmptyQueryRequest());
        
    }

}
