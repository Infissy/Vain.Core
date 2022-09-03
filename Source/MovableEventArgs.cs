using System;
using Godot;


namespace Vain
{
    public class CollisionEventArgs : EventArgs
    {   

        
        public Node Collider {get; private set;} 
        public CollisionEventArgs(Node collider) : base() => Collider = collider;
    }

     public class MovementUpdateEventArgs : EventArgs
    {   

        
        public Vector3 Position {get; private set;} 
        public MovementUpdateEventArgs(Vector3 position) : base() => Position = position;
    }
}