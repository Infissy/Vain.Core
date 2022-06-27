
using Godot;


namespace Vain
{
    

    public class Health : Component
    {

       
        float _maxhealth;
        
        float _currentHealth;

        public float CurrentHealth => _currentHealth;

        public float MaxHealth => _maxhealth;
      
        public void Heal(int amount){


            _currentHealth = _currentHealth + amount > _maxhealth ? _maxhealth : _currentHealth + amount;
            
            
        }

        public void Damage(float amount){
            _currentHealth = _currentHealth - amount;
            
        
            if(_currentHealth <= 0){
                ComponentEntity.Kill();
            }


            

        }

        //Implement Entity destruction
    }
}