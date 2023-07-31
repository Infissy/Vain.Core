using System;
using Godot;
using Vain.Core;
using Vain.Singleton;
namespace Vain.InteractionSystem
{

    //Class that handles interaction between characters, quests and any events that can affect gameplay
   
    public partial class InteractionHandler : Node
    {
        public override void _EnterTree()
        {
            base._EnterTree();
            SingletonManager.Register(this);
        }

        public override void _Ready()
        {
            base._Ready();

            GD.Print(SingletonManager.GetSingleton<InteractionHandler>());
        }
    

    }    
}