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
            
            var player = SingletonManager.GetCharacterSingleton(SingletonManager.Singletons.PLAYER);
            var healthComponent = player.GetComponent<HealthComponent>();
            
            healthComponent.RegisterToSignal(HealthComponent.SignalName.HealthUpdate,new Callable(this,MethodName.UpdateHealthHandler));


          
        }

        public void UpdateHealthHandler(float health)
        {
            Health = health;
        }

       


        
    } 
}