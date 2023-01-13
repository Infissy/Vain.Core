using Godot;
using Vain.UI;
namespace Vain
{
    partial class CharacterHealthBar : Component
    {

        HealthBar _bar;

        public override void _Ready()
        {
            base._Ready();
            _bar = GetNode<HealthBar>("SubViewport/HealthBar");
            var health = GetComponent<Health>();
            _bar.MaxHealth = health.MaxHealth;
            _bar.Health = health.CurrentHealth;

            health.HealthUpdate += (health) => _bar.Health = health;
            

        
        }









    }
}