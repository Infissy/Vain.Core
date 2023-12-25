using Godot;
using System.Collections.Generic;
using Vain.Log;
using Vain.SpellSystem;
// Effectable component for a entity to be able to get an effect from a spell


//TODO: Refactor 
namespace Vain.Core.ComponentSystem;

[GlobalClass]
partial class EffectableComponent : Component
{
    readonly List<Effect> _effects = new();
    readonly List<Effect> _effectsApplied = new();

    [RequiredComponent]
	MovementControllerComponent _characterController;

	[RequiredComponent]
	HealthComponent _health;

	public override void _Ready()
	{
		base._Ready();
		_characterController = GetComponent<MovementControllerComponent>();
		_health = GetComponent<HealthComponent>();
	}
	public void ApplyEffects(List<Effect> effectsToApply)
	{
		_effects.AddRange(effectsToApply);
	}

	public override void _Process(double delta)
	{
		if(_effects.Count > 0 || _effectsApplied.Count > 0)
			HandleEffects((float)delta);
	}

	void HandleEffects(float delta){
		for (int i = 0; i < _effects.Count; i++)
		{
			switch(_effects[i].effectType){
				case EffectType.DAMAGE:
					//Logger.SetContext(Character).Debug("Entity Damaged");
					_health?.Damage(_effects[i].Value);
					break;
				case EffectType.SPEEDBOOST:
					if(_characterController != null)
						_characterController.SpeedModifier = _effects[i].Value;

					break;
			}

			Effect eff = _effects[i];
			eff.Duration -= delta;
			_effects[i] = eff;
			_effectsApplied.Add(_effects[i]);
			_effects.RemoveAt(i);
		}

		for (int i = 0; i < _effectsApplied.Count; i++)
		{
			if(_effectsApplied[i].Duration >= 0)
			{
				Effect eff = _effectsApplied[i];
				eff.Duration -= delta;
				RuntimeInternalLogger.Instance.SetContext(Character).Debug(eff.Duration.ToString());
				_effectsApplied[i] = eff;
				return;
			}

			switch (_effectsApplied[i].effectType)
			{
				case EffectType.SPEEDBOOST:
				if (_characterController != null)
					_characterController.SpeedModifier = 1-_effectsApplied[i].Value;
				break;
			}
			_effectsApplied.RemoveAt(i);
		}
	}
}
