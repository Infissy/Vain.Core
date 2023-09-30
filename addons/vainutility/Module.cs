#if TOOLS
using Godot;

namespace Vain.Plugins.VainUtility
{
    abstract internal partial class Module : Control
    {
        internal abstract string GetPluginName();
    }
}
#endif