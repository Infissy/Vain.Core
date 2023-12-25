using Vain;
using Godot;
using Vain.Configuration;

namespace Vain.CLI;

public static class ScriptLoader
{
    public static Script LoadScript(string scriptName)
    {
        foreach (var path in ProjectConfiguration.LoadConfiguration(ProjectConfiguration.MultiSourceConfiguration.ScriptsFolder))
        {
            var scriptFile = FileAccess.Open($"{path}/{scriptName}",FileAccess.ModeFlags.Read);
            if(scriptFile  != null)
            {
                var text = scriptFile.GetAsText();
                return new Script(text);
            }
        }

        return null;
    }
}
