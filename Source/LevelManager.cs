using System.Collections.Generic;
using Godot;

using Vain.Singleton;
namespace Vain.Core
{
    //Should handle all the resources inside a single map/level
    
    
    public partial class LevelManager : Node
    {
        List<Character> _characters = new List<Character>();

        public Character[] Characters {get {return _characters.ToArray();}}


        public IEntity [] Entities {get;private set;}

        
        public override void _EnterTree()
        {
            base._EnterTree();
            SingletonManager.Register(this);
        }


    }
}