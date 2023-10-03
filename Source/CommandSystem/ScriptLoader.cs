using Godot;
namespace Vain.CLI
{
    public static class ScriptLoader 
    {
        public static Script LoadScript(string scriptName)
        {

            if(!FileAccess.FileExists($"res://Resources/Game/Scripts/{scriptName}"))
                return null;
           
            
           
            var text = FileAccess.Open($"res://Resources/Game/Scripts/{scriptName}",FileAccess.ModeFlags.Read).GetAsText();
            var script = new Script(text);

            return script;
            
        }
    } 
}