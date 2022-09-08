using Godot;
using Vain.UI;

namespace Vain
{


    class HealthBar : Component , IInitialize, ILateInitializable
    {
        
        Health _health;
        HealthBarUI _healthBarUI;

        Movable _movable;


        public void Initialize()
        {
            
            _health = GetComponent<Health>();
            _movable = GetComponent<Movable>();

        }




        public void LateInitialize()
        {
             _healthBarUI = UI.UI.Register<HealthBarUI>();

            _healthBarUI.Health = _health.CurrentHealth;
        }


        public void PhysicsProcess(float deltatime)
        {
            //TODO: 3D UI Positioning
            //_healthBarUI.Position = _movable.Position;
        }

        
    }


}