using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using Vain.Console;
// * Rappresents every contained entity in the game in which components are applied


namespace Vain
{

    
    public class Entity : Node 
    {
    
        


        
        


        
        static Stack<int> OldIDs = new Stack<int>();
        static int idCount = 0;
        static List<Entity> _entities  = new List<Entity>();
        int _id;
        ComponentContainer _container;
        float _delta;



        public ComponentContainer ComponentContainer => _container;
        public static List<Entity> Entities {get => _entities;}

        public int ID {get  => _id;}

        

        


        static int Register(Entity entity){

            if(OldIDs.Count > 0)
                return OldIDs.Pop();

            _entities.Add(entity);            

            return idCount ++;

        }

        static void Unregister( int id, Entity entity)
        {

            _entities.Remove(entity);
            OldIDs.Push(id);

        }


        
        Entity()
        {
            _id = Register(this);
        }

        public override void _Ready()
        {
            base._Ready();
            
            _container = GetChildren().OfType<ComponentContainer>().First();
            
        
        }


        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
        
            _delta = delta;
        }


        public Node Query (Type nodeType) 
        {
            foreach (var child in GetChildren())
            {
                if(child.GetType() == nodeType)
                    return child as Node;
            }

            return null;
        }

        public void Kill()
        {
            foreach (var killListener in ComponentContainer.Components.OfType<IKillListenable>())
            {
                killListener.OnKill();
            }


            QueueFree();
        }
        public override void _ExitTree()
        {
            base._ExitTree();


            Unregister(_id,this);
        }
        

    
            

    }

}