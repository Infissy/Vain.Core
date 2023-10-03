using Godot;
using System;
namespace Vain.InteractionSystem.InteractionGraph
{
    [GlobalClass]
    public partial class ConfigurationResource : Resource
    {
        [Export]
        public string CharacterFolder {get;set;}
        
    }
}
