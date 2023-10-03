using Godot;
using Godot.Collections;

namespace Vain.InteractionSystem
{
    [GlobalClass]
    public partial class WorldNPCInfo : Resource
    {
        [Export]
        public string Name {get;set;}
        [Export]
        public string Description {get;set;}



        //TODO: Export mesh and other custom components in a behaviour resource, so its separated and it's possible to have an initialization routine tailored for custom properties
        [Export]
        public Mesh CustomMesh {get;set;}


        [Export]
        Array<Interaction> Dialogues {get;set;}
    }
}