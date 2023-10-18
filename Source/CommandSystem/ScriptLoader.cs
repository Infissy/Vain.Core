using Vain;
using Godot;
namespace Vain.CLI
{
    public static class ScriptLoader 
    {
        public static Script LoadScript(string scriptName)
        {

            var paths = ProjectConfig.LoadConfiguration(ProjectConfig.MultiSourceConfiguration.ScriptsFolder);
            
            foreach (var path in paths)
            {
                var scriptFile = FileAccess.Open($"{path}/{scriptName}",FileAccess.ModeFlags.Read);
                if(scriptFile  != null)
                {
                    var text = scriptFile.GetAsText();
                    var script = new Script(text);
                    return script;
                }
            }
            
           
            return null;
            
        }
    } 
}