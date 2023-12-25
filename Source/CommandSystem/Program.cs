using System.Collections.Generic;

namespace Vain.CLI;
public class Program
{
    public string Name {get;set;} = "default";

    public string Description {get;set;} = "";

    public List<Command> Commands { get; } = new List<Command>();

    public string Run(string[] parameters)
    {
        object[] parsedParameters = null;
        Command workingCommand = null;
        foreach (var command in Commands)
        {
            if(!command.TryParseParameters(parameters,out parsedParameters))
                continue;


            workingCommand = command;
            break;
        }

        if(workingCommand == null)
            throw new InvalidParameterException();
        var result =  workingCommand.Action.DynamicInvoke(parsedParameters);
        return result == null ? "" : result as string;
    }
}



