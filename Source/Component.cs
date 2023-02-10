using System.Linq;
using Godot;

namespace Vain
{
    public partial class Component : Node
    {
        
        
        public Character Character { get; private set; }



        
        public override void _Ready()
        {
            base._Ready();
            Character = GetParent<Character>();
        }

        public T GetComponent<T>(bool optional = false) where T : Component
        {

            

            //In case we want a component before _ready has been called
            if(Character == null)
                Character = GetParent<Character>();
            
            return Character.GetComponent<T>(optional);


            
        }
    }
}