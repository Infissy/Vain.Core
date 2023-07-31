using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.Singleton;

namespace Vain.UI
{

    public partial class PlayerHealthBar : HealthBar
    {
     

        public override void _Ready()
        {
            base._Ready();
            
            var player =  SingletonManager.GetSingleton<Player>();
            var healthComponent = player.CurrentCharacter.GetComponent<HealthComponent>();

            base.Health = healthComponent.CurrentHealth;
            base.MaxHealth = healthComponent.MaxHealth;
           
            healthComponent.HealthUpdate += updateHealthHandler;

            player.CurrentCharacterChanged += (oldCharacter) =>
            {
                oldCharacter.GetComponent<HealthComponent>().HealthUpdate -= updateHealthHandler;
                player.CurrentCharacter.GetComponent<HealthComponent>().HealthUpdate += updateHealthHandler;
            };
            
            
        }

        void updateHealthHandler(float health)
        {
            Health = health;
        }

       


        
    } 
}