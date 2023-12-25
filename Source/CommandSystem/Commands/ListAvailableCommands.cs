
using Vain.Log;

namespace Vain.CLI;
public static partial class DefaultPrograms
{
     public static readonly Program ListAvailableCommands = new()
    {
        Name  = "?",

        Commands =
        {
            new Command
            (
                "",
                ()=>{
                    var runner = CommandRunner.Instance;
                    foreach (var command in CommandRunner.Instance.Commands)
                    {
                        RuntimeInternalLogger.Instance.Command(command.Name,command.Description, false);
                    }
            })
        }
    };
}