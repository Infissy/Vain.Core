using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vain.Core;
using Vain.Core.ComponentSystem;

namespace Vain.Singleton
{
    public class SingletonCharacter : Singleton<Character>
    {
        Dictionary<Type,SingletonHandle> _components = new Dictionary<Type, SingletonHandle>();

        public Singleton<C>? GetComponent<C>() where C : Component
        {
            if(_components.ContainsKey(typeof(C)))
                return _components[typeof(C)] as Singleton<C>;


        

            var component = Reference.GetComponent<C>();
            
            if(component == null)
                return null;

            
            
            Singleton<C> singletonComponent = new Singleton<C>(component);
            

            _components[typeof(C)] = singletonComponent;


            return _components[typeof(C)] as Singleton<C>;
        }


        protected override void updateReference(Character newReference)
        {
            base.updateReference(newReference);


            foreach (var component  in _components.ToList())
            {




                var newComponent = newReference.GetComponent(component.Key);                
                var reference = component.Value as Singleton<Component>;



                if(newComponent == null)
                    reference.Disposed = true;
                else
                    reference.Reference = newComponent;
                

            
            }

        }

        public SingletonCharacter (Character instance) : base (instance)
        {

        }
    }
}