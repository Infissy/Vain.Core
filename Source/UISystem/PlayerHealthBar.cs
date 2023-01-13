using Godot;
namespace Vain.UI
{
    public partial class PlayerHealthBar : HealthBar
    {
        PlayableGame _gameManager;


        public override void _Ready()
        {
            base._Ready();
            //TODO: Maybe do it with reflection to avoid code repetition
            _gameManager = GetNode<PlayableGame>("/root/Game/PlayableGame");       
            _gameManager.CharacterChanged += playingCharacterChanged;

            
        }

        void playingCharacterChanged(Character oldCharacter)
        {
            if (oldCharacter != null)
            {
                oldCharacter.GetComponent<Health>().HealthUpdate -= (health) => base.Health = health;

            }

            var healthComponent = _gameManager.PlayableCharacter.GetComponent<Health>();
            base.Health = healthComponent.CurrentHealth;
            base.MaxHealth = healthComponent.MaxHealth;
            healthComponent.HealthUpdate += (health) => base.Health = health;
            

        }


        
    } 
}