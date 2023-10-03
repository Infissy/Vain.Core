using Godot;

namespace Vain.Core
{
    [GlobalClass]
    public partial class IndexedResourceWrapper : Resource
    {
        [Export]
        public GodotObject Resource {get;set;}


        public Node Instantiate()
        {
            if(Resource is PackedScene scene)
                return scene.Instantiate();
            if(Resource is CSharpScript script)
                return (Node) script.New();
            return null;
        }
    }
}