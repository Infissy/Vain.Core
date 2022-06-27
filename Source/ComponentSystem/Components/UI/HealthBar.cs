using Godot;
using Vain.UI;

namespace Vain
{


    class HealthBar : Component
    {
        
        Health _health;
        HealthBarUI _healthBarUI;


        public override void _Ready()
        {
            base._Ready();
            _health = GetParent<ComponentContainer>().GetComponent<Health>();
        }

        public void LateInitialize()
        {
             _healthBarUI = UI.UI.Register<HealthBarUI>();

            _healthBarUI.Health = _health.CurrentHealth;
        }


        public override void _PhysicsProcess(float deltatime)
        {
            
            _healthBarUI.Position = (Owner as KinematicBody2D).Position;
        }

        
    }


}