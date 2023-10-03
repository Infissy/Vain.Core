#if TOOLS
using Godot;


namespace Vain.Plugins.VainUtility.GameIndex
{
    internal partial class EntityIndexGenerationButton : ButtonModule
    {

        public override void _Ready()
        {
            base._Ready();
            ButtonName = "Generate Entity Index";
        }
        internal override string GetPluginName()
        {
            return "GameIndexUtility";
        }
        void LoadEntities()
        {
            
        }
    }
}
#endif