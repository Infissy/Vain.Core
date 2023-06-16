using System;
using System.Linq;
using System.Collections.Generic;

using Godot;

using Vain.Core;


namespace Vain
{

    /// <summary>
    /// Character is the base class for all Characters in the game. Any special characters to be considered as such and have any component need a Character parent.
    /// </summary>
        public abstract partial class Character : CharacterBody3D , IEntity
    {
        
        


        List<Component> _components;

        public bool ActionLock {get; private set;}

        public uint RuntimeID {get; protected set;}

        [Signal]
        public delegate void CharacterKilledEventHandler();


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

        public T GetComponent<T>(bool optional = false) where T : Node
        {   

            //In case we need a preload, although components in this case won't be ready, so might need some sort of warning
            if(_components == null)
                loadComponents();
            
          

            if(optional)
                return _components.Where((c) => c is T).FirstOrDefault() as T;

            return _components.Where(c => c is T).First() as T;
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
                if(node is Component)
                    _components.Add(node as Component);
            }
            

        }

    
    }
}