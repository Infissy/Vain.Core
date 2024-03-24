using Godot;

namespace Vain.HubSystem;

//TODO: Might not be the proper name, rename in case something better comes
public partial class HubAnchor : Node
{
    public override void _Process(double delta)
    {
        base._Process(delta);

        Hub.Instance.ConsumeEvents();
    }
}