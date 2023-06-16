using System.Collections.Generic;
using System.Linq;
using Godot;
using Vain.Core;

namespace Vain.InteractionSystem
{
 
    //TODO: At the moment all the system is focused to the interaction between NPC and Player, maybe allow NPC to NPC interaction for more complex social behaviours 
    public partial class Interactor : Component
    {
        const string ACTION = "player_interact";



        //TODO: Think about 3D dialogue so interaction between characters can be displayed
        [Signal]
        public delegate void OnDialogueEventHandler(string dialogue);

   
        Area3D _area;

        InteractionHandler _system;
        List<Interactible> _interactibles = new List<Interactible>();
        
        public override void _Ready()
        {
            base._Ready();
            
            _area = GetChild<Area3D>(0);

            _area.BodyEntered += _bodyEntered;
            _area.BodyExited += _bodyExited;
            

           
            _system = SingletonManager.GetSigletonOrDefault<InteractionHandler>();
            
            
            //Disable component if no interactionHandler is available 
            if(_system == null)
                this.ProcessMode = ProcessModeEnum.Disabled;
        }
        



        void _bodyEntered(Node body)
        {
            if(body is NPC npc)
            {   var interactible = npc.GetComponent<Interactible>(true);
                
                if(interactible != null)
                    _interactibles.Add(interactible);
                
            }
        }


        void _bodyExited(Node body)
        {
            if(body is NPC npc)
            {
                var interactible = npc.GetComponent<Interactible>(true);
                
                if(interactible != null)
                    _interactibles.Remove(interactible);

                
            }
        }


        
        public override void _UnhandledInput(InputEvent inputEvent)
        {
        
            
            base._UnhandledInput(inputEvent);

            
            if (inputEvent is InputEventKey key)
            {
                if(Input.IsActionJustPressed(ACTION) && _interactibles.Count > 0)
                {
                    
                    Interactible nearestInteractible = _interactibles.OrderBy<Interactible,float>(
                                    i => i.Character.GlobalPosition.DistanceTo(this.Character.GlobalPosition))
                                    .First();

                    var interaction = nearestInteractible.Interact();

                    handleInteraction(interaction);
                }  

            }

       }



        void handleInteraction(Interaction interaction)
        {
            if(interaction is DialogueInteraction dialogueInteraction)
            {
            
                EmitSignal(SignalName.OnDialogue,dialogueInteraction.Dialogue);
            }
        }
    }
    
}