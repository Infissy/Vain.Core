using System;
using Godot;

namespace Vain.InteractionSystem
{

    //Class that handles interaction between characters, quests and any events that can affect gameplay
    [Singleton]
    public partial class InteractionHandler : Node
    {
    

        public override void _Ready()
        {
            base._Ready();

            GD.Print(SingletonManager.GetSigletonOrDefault<InteractionHandler>());
        }
    

    }    
}