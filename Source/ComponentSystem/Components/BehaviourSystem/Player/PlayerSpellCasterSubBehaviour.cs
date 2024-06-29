using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

using Vain.Core.ComponentSystem;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using Vain.Log;
using static Vain.HubSystem.GameEvent.GameEvents.Spell;
using static Vain.HubSystem.Query.Queries;

namespace Vain.SpellSystem;
public partial class PlayerSpellCasterSubBehaviour : SubBehaviour,
	IListener<PlayerSpellInputEvent,PlayerSpellInputEventArgs>
{

	

	public override void _Ready()
	{

		base._Ready();
		
		
		
		Hub.Instance.Subscribe(this);
		
		var response = Hub.Instance.QueryData<SpellPathQuery,EmptyQueryRequest,SpellPathQueryResponse>(new EmptyQueryRequest());
	
		if(response != null)
			BehaviourComponent.Character.GetComponent<SpellCasterComponent>().Spells = response?.NextLayerTemplates;
	}
	
	int _lastInputCount	= 0;
	List<SpellInput> _inputs = new List<SpellInput>();

	private static class ACTIONS
	{
		public const string UP = "player_cast_up";
		public const string DOWN = "player_cast_down";
		public const string LEFT = "player_cast_left";
		public const string RIGHT = "player_cast_right";
		public const string CAST = "player_cast";
	}


    public override void _Process(double delta)
    {
        base._Process(delta);

		if(_inputs.First() == SpellInput.EnterCast && _inputs.Last() == SpellInput.ExitCast && _inputs.Count > 0)
		{
			Cast();
			_inputs.Clear();
			_lastInputCount = 0;
			return;
		}
	

		if(Input.IsActionJustPressed(ACTIONS.CAST))
			_inputs.Add(SpellInput.EnterCast);
			
		if(Input.IsActionJustPressed(ACTIONS.LEFT))
			_inputs.Add(SpellInput.Left);
		if(Input.IsActionJustPressed(ACTIONS.RIGHT))
			_inputs.Add(SpellInput.Right);
		if(Input.IsActionJustPressed(ACTIONS.UP))
			_inputs.Add(SpellInput.Top);
		if(Input.IsActionJustPressed(ACTIONS.DOWN))
			_inputs.Add(SpellInput.Down);	
		if(Input.IsActionJustReleased(ACTIONS.CAST))
			_inputs.Add(SpellInput.ExitCast);

		if(_inputs.Count > _lastInputCount)
		{
			_lastInputCount = _inputs.Count;
			Hub.Instance.Emit<PlayerSpellInputEvent,PlayerSpellInputEventArgs>(new PlayerSpellInputEventArgs{Input = _inputs.Last()});
		}
		

		
		
	}
  
	void Cast(){
	


		var caster = BehaviourComponent.Character.GetComponent<SpellCasterComponent>();
		var spellPath = Hub.Instance.QueryData<SpellPathQuery,EmptyQueryRequest,SpellPathQueryResponse>(new EmptyQueryRequest());

		Debug.Assert(spellPath != null);	
		_inputs.RemoveAt(0);
		_inputs.RemoveAt(_inputs.Count - 1);

		if(_inputs.Count * 4 < spellPath?.FirstLayer.Count )
			return;


		
		int spellRange = spellPath?.FirstLayer.Count ?? 0 / 4;



		//Input spell as base4 sequence
		int spellIndex = 0;
		for (int i = 0; i < spellRange; i++)
		{
			spellIndex += ((int)_inputs[i]) * 4 ^ i;
		}

		var spell = spellPath?.FirstLayer.ElementAtOrDefault(spellIndex);
		if(spell == null)
			return;

		int templateIndex = 0;
		for (int i = 0; i < spellPath?.NextLayerTemplates[spell]?.Length - spellRange; i++)
		{
			templateIndex += ((int)_inputs[spellRange + i]) * 4 ^ i;
		}

		var template = spellPath?.NextLayerTemplates[spell][templateIndex];


		var response = Hub.Instance.QueryData<MousePositionQuery,EmptyQueryRequest,PositionQueryResponse>(new EmptyQueryRequest());		
		caster.CastSpell(spell, template,response?.Position ?? BehaviourComponent.Character.Position);

		RuntimeInternalLogger.Instance.Debug($"Player cast {spell} {template}");
	}

    public void HandleEvent<E>(PlayerSpellInputEventArgs args)
    {
        RuntimeInternalLogger.Instance.Debug($"Input: {args.Input}");
    }
}
