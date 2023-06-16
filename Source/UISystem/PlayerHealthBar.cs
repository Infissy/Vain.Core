using Godot;
using Vain.Core;

namespace Vain.UI
{

    public partial class PlayerHealthBar : HealthBar
    {
     

        public override void _Ready()
        {
            base._Ready();
            
            var healthComponent = SingletonManager.GetSingleton<Player>().GetComponent<Health>();
            base.Health = healthComponent.CurrentHealth;
            base.MaxHealth = healthComponent.MaxHealth;
            healthComponent.HealthUpdate += (health) => base.Health = health;
            
        }

       


        
    } 
}