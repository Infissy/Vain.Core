using System;
using System.Linq;
using System.Collections.Generic;

using Godot;

using Vain.Core.ComponentSystem;
using Vain.Singleton;

namespace Vain.Core
{

    /// <summary>
    /// Character is the base class for all Characters in the game. Any special characters to be considered as such and have any component need a Character parent.
    /// </summary>
    public partial class Character : CharacterBody3D , IEntity
    {
        


        List<Component> _components;

        CharacterBehaviourComponent _behaviour;

        [Export] 
        public CharacterBehaviourComponent CharacterBehaviour 
        {   
            get => _behaviour;
            set
            {
                var oldBehaviour = _behaviour;

                _behaviour = value;

                
            }
        }
        
        public bool ActionLock {get; private set;}

        public uint RuntimeID {get; protected set;}

        [Signal]
        public delegate void CharacterKilledEventHandler();
        [Signal]
        public delegate void CharacterBehaviourUpdateEventHandler(CharacterBehaviourComponent OldBehaviour);


      
        
        public override void _Ready()
        {
            base._Ready();

            if(_components == null)
                loadComponents();

            SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference?.Register(this);
            
        

        }


        public void AddComponent(Component component)
        {
            AddChild(component);

            _components.Add(component);
        }


       

        public T? GetComponent<T>() where T : Component
        {   

            //In case we need a preload, although components in this case won't be ready, so might need some sort of warning
            if(_components == null)
                loadComponents();
       
            return _components.Where((c) => c is T).FirstOrDefault() as T;

        }
         
        public Component GetComponent(Type type) 
        {   

            //In case we need a preload, although components in this case won't be ready, so might need some sort of warning
            if(_components == null)
                loadComponents();
       
            return _components.Where((c) => c.GetType() == type).FirstOrDefault();

        }

        public IReadOnlyCollection<Component> GetComponents()
        {
            return _components.AsReadOnly();
        }

        public virtual void Kill()
        {

            EmitSignal(SignalName.CharacterKilled);
           
            this.QueueFree();



        }



        void loadComponents()
        {
        
            _components = new List<Component>();


            foreach ( var node in GetChildren())
            {
                if(node is Component component)
                    _components.Add(component);
            }
            

        }

    
    }
}