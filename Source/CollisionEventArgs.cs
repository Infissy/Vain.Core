using System;
using Godot;


namespace Vain
{
    public class CollisionEventArgs : EventArgs
    {   

        
        public Node Collider {get; private set;} 
        public CollisionEventArgs(Node collider) : base() => Collider = collider;
    }
}