using Godot;
using System.Linq;

namespace Vain
{

    //TODO: ? maybe rename, bad name
    public partial class PlayableGame : Node
    {  
 
        

        
        //Passing value so various events can unsubscribe or do stuff before descarding the old character
        [Signal]
        public delegate void CharacterChangedEventHandler(Character oldCharacter);
        
        Character _character;
    


        //Late initialization, defaults to player unless you change to something else. Emits signal so everything bound to current character can reload to new character



        public Character PlayableCharacter 
        {
            
            get
            {
                if(_character == null)
                    _character = GetChildren().OfType<Node3D>().First().GetNode<Player>("Player");

                return _character;
            }
            
            set
            {   
                var oldCharacter = _character;
                _character = value;
                EmitSignal(nameof(CharacterChanged),oldCharacter);

            }
            
            
        } 
        


        public override void _Ready()
        {
            base._Ready();

            //TODO: Get child should theorically get the map node, find a better solution less reliant on hierarchy
            var player = GetChildren().OfType<Node3D>().First().GetNode<Player>("Player");
            PlayableCharacter = player;

        }

   




    }
}