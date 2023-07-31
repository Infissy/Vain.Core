using System;
using System.Linq;
using System.Collections.Generic;

using Godot;

using Vain.Core.ComponentSystem;


namespace Vain.Core
{

    /// <summary>
    /// Character is the base class for all Characters in the game. Any special characters to be considered as such and have any component need a Character parent.
    /// </summary>
    public partial class Character : CharacterBody3D , IEntity
    {
        


        List<Component> _components;

        CharacterBehaviour _behaviour;

        [Export] 
        public CharacterBehaviour CharacterBehaviour 
        {   
            get => _behaviour;
            set
            {
                var oldBehaviour = _behaviour;

                _behaviour = value;

                EmitSignal(SignalName.CharacterBehaviourUpdate);
            }
        }
        
        public bool ActionLock {get; private set;}

        public uint RuntimeID {get; protected set;}

        [Signal]
        public delegate void CharacterKilledEventHandler();
        [Signal]
        public delegate void CharacterBehaviourUpdateEventHandler(CharacterBehaviour OldBehaviour);


        public override void _EnterTree()
        {
            base._EnterTree();
            
        }
        
        
        
        public override void _Ready()
        {
            base._Ready();

            if(_components == null)
                loadComponents();
            
            //SingletonManager.GetSingleton<LevelManager>().AddCharacter(this);
            
        

        }


        public void AddComponent(Component component)
        {
            AddChild(component);

            _components.Add(component);
        }


        public void Lock()
        {
            
        }

        public T? GetComponent<T>() where T : Node
        {   

            //In case we need a preload, although components in this case won't be ready, so might need some sort of warning
            if(_components == null)
                loadComponents();
            
          

           
            return _components.Where((c) => c is T).FirstOrDefault() as T;

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