using Vain;
using Godot;


namespace Vain.CLI;

public static class ScriptLoader
{

    static readonly string[] s_ScriptFolders = { "res://Vain.Core/Resources/Scripts", "res://Vain/Resources/Scripts" };

    public static Script? LoadScript(string scriptName)
    {
        foreach (var path in s_ScriptFolders)
        {
            var scriptFile = FileAccess.Open($"{path}/{scriptName}", FileAccess.ModeFlags.Read);
            if (scriptFile != null)
            {
                var text = scriptFile.GetAsText();
                return new Script(text);
            }
        }

        return null;
    }
}
