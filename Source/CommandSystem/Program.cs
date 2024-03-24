using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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


        object result = null;

        if (workingCommand.Action.Method.IsDefined(typeof(AsyncStateMachineAttribute), false))
        {
            dynamic tmp = workingCommand.Action.DynamicInvoke(parsedParameters);
           
            
            if(workingCommand.Action.Method.ReturnType == typeof(Task<string>))
                result = (tmp as Task<string>).Result;
            }
        else
            result =  workingCommand.Action.DynamicInvoke(parsedParameters);
           
        
        return result == null ? "" : result as string;
    }
}



