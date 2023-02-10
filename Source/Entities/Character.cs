using System;
using System.Linq;
using System.Collections.Generic;

using Godot;

namespace Vain
{

    
    public partial class Character : CharacterBody3D 
    {
        
 
        List<Component> _components;

        
        
        
        [Signal]
        public delegate void CharacterKilledEventHandler();


   
        
        
        
        public override void _Ready()
        {
            base._Ready();

            if(_components == null)
                loadComponents();
            
            
        

        }


        public void AddComponent(Component component)
        {
            AddChild(component);

            _components.Add(component);
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

            EmitSignal(nameof(CharacterKilled));
           
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