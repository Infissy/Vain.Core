using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Godot;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using Vain.HubSystem.Query;
using Vain.Log;
using Vain.SpellSystem.Aspects;
using static Vain.HubSystem.GameEvent.GameEvents.Spell;
using static Vain.HubSystem.Query.Queries;
namespace Vain.SpellSystem;

partial class SpellSystem : Node,
    IListener<SpellCastEvent, SpellCastEventArgs>,
    IDataProvider<SpellPathQuery,EmptyQueryRequest, SpellPathQueryResponse>
{
    Dictionary<string,string[]> _templates = new();
    Dictionary<string, Type> _aspects = new();


    public override void _Ready()
    {
        base._Ready();
        Hub.Instance.RegisterDataProvider(this);
        Hub.Instance.Subscribe(this);

        AddAspect<FireballAspect>();
    }
    
    public void AddAspect<T>() where T : Aspect {

        var spellName = Aspect.GetName<T>();
        var templates = Aspect.GetTemplates<T>();

        _aspects.Add(spellName,typeof(T));
        
        _templates[spellName] = templates;

    }



    public void HandleEvent<E>(SpellCastEventArgs args)
    {

        RuntimeInternalLogger.Instance.Debug($"Spell cast: {args.SpellName} | {args.Template} by {args.Caster} to {args.Target}");


        var aspect = _aspects[args.SpellName];   

        var spell = new Spell(args.Caster, Activator.CreateInstance(aspect) as Aspect, args.Target); 

        AddChild(spell);
        spell.Position = args.Caster.GlobalPosition;
        
    }


    public SpellPathQueryResponse? Provide(EmptyQueryRequest request)
    {
        return new SpellPathQueryResponse{
            FirstLayer = _aspects.Keys.ToList(),
            NextLayerTemplates = _templates,
        };
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        Hub.Instance.UnregisterDataProvider(this);
        Hub.Instance.Unsubscribe(this);
    }

}
