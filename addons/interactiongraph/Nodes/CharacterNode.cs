using Godot;
using System.Collections.Generic;

namespace Vain.InteractionSystem.InteractionGraph
{
    

    [Tool]
    public partial class CharacterNode : GraphNode
    {
        public void SetCharacters(List<string> names)
        {
            var optionButton = GetChild<OptionButton>(0);
            optionButton.Clear();
            
            foreach (var name in names)
            {
                GD.Print(name);
                optionButton.AddItem(name);
                
            }
        }
    } 


}