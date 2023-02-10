using Godot;
namespace Vain.UI
{
    public partial class PlayerHealthBar : HealthBar
    {
        PlayableGame _playableGame;


        public override void _Ready()
        {
            base._Ready();
            //TODO: Maybe do it with reflection to avoid code repetition
            _playableGame = GetNode<PlayableGame>("/root/Game/PlayableGame");       
            
            var healthComponent = SingletonManager.GetSingleton<Player>().GetComponent<Health>();
            base.Health = healthComponent.CurrentHealth;
            base.MaxHealth = healthComponent.MaxHealth;
            healthComponent.HealthUpdate += (health) => base.Health = health;
            
        }

       


        
    } 
}