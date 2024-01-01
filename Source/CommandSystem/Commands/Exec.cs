
using Vain.Log;

namespace Vain.CLI;

public static partial class DefaultPrograms
{

     public static readonly Program Exec = new()
    {

        Name = "exec",

        Commands =
        {
            new Command
            (
                "?:s",
                (string scriptName) =>
                {
                    var script = ScriptLoader.LoadScript(scriptName.Contains(".cfg") ? scriptName : scriptName + ".cfg");

                    if(script == null)
                    {
                        RuntimeInternalLogger.Instance.Warning($"Script {scriptName} does not exist.");
                        return;
                    }
                    script.Run();
                }
            )
        }
    };

}