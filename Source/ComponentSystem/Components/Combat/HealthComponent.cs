using Godot;

//Health component, for a entity to die after health gets to 0
namespace Vain.Core.ComponentSystem;

[GlobalClass]
public partial class HealthComponent : Component
{
    [Export]
    public float CurrentHealth { get; private set; }

    [Export]
    public float MaxHealth { get; private set; }

    //At the moment useful only for UI, maybe differenciate between heal and damage?
    [Signal]
    public delegate void HealthUpdateEventHandler(float health);

    public override void _Ready()
    {
        base._Ready();
        CurrentHealth = MaxHealth;
    }
    public void Heal(int amount)
    {
        CurrentHealth = CurrentHealth + amount > MaxHealth ? MaxHealth : CurrentHealth + amount;
        EmitSignal(SignalName.HealthUpdate, CurrentHealth);
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;
        EmitSignal(SignalName.HealthUpdate, CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Character.Kill();
        }
    }
}
