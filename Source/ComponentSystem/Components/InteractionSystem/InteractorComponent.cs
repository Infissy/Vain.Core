using System.Collections.Generic;
using System.Linq;
using Godot;
using Vain.Core;
using Vain.Core.ComponentSystem;
using Vain.Singleton;

namespace Vain.InteractionSystem
{
 
    //TODO: At the moment all the system is focused to the interaction between NPC and Player, maybe allow NPC to NPC interaction for more complex social behaviours 
    public partial class InteractorComponent : Component
    {
    


        //TODO: Think about 3D dialogue so interaction between characters can be displayed
        [Signal]
        public delegate void OnDialogueEventHandler(string dialogue);

   
        Area3D _area;

        Singleton<InteractionHandler> _system;
        List<InteractibleComponent> _interactibles = new List<InteractibleComponent>();
        
    

        public override void _Ready()
        {
            base._Ready();
            
            _area = GetChild<Area3D>(0);

            _area.BodyEntered += _bodyEntered;
            _area.BodyExited += _bodyExited;

            _system = SingletonManager.GetSingleton<InteractionHandler>(SingletonManager.Singletons.INTERACTION_HANDLER);

            //Disable component if no interactionHandler is available 
            if(_system == null)
                this.ProcessMode = ProcessModeEnum.Disabled;
        }
        



        void _bodyEntered(Node body)
        {
            if(body is Character character)
            {   var interactible = character.GetComponent<InteractibleComponent>();
                
                if(interactible != null)
                    _interactibles.Add(interactible);
                
            }
        }


        void _bodyExited(Node body)
        {
            if(body is Character character)
            {
                var interactible = character.GetComponent<InteractibleComponent>();
                
                if(interactible != null)
                    _interactibles.Remove(interactible);

                
            }
        }


        
        void CharacterActionHandler(CharacterAction action)
        {

            if(action == CharacterAction.INTERACT && _interactibles.Count > 0)
            {
                
                InteractibleComponent nearestInteractible = _interactibles.OrderBy<InteractibleComponent,float>(
                                i => i.Character.GlobalPosition.DistanceTo(this.Character.GlobalPosition))
                                .First();

                var interaction = nearestInteractible.Interact();

                handleInteraction(interaction);
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