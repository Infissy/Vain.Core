using Godot;
namespace Vain.CLI
{
    public static class ScriptLoader 
    {
        public static Script LoadScript(string scriptName)
        {

            if(!FileAccess.FileExists($"{GameData.Folders.ScriptFolder}/{scriptName}"))
                return null;
           
            
           
            var text = FileAccess.Open($"{GameData.Folders.ScriptFolder}/{scriptName}",FileAccess.ModeFlags.Read).GetAsText();
            var script = new Script(text);

            return script;
            
        }
    } 
}