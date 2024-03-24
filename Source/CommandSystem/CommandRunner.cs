using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Vain.Log;

namespace Vain.CLI;

///<summary>
/// Main Class that handles command execution
///</summary>
internal class CommandRunner
{
    readonly Dictionary<string, Program> _commands = new();
    
    readonly ConcurrentDictionary<string,string> _variables = new();
    readonly ConcurrentQueue<(Program, string[],string)> _commandQueue = new();


    

    public Program[] Commands  => _commands.Values.ToArray();
    public static CommandRunner Instance { get; } = new();

    public CommandRunner()
    {
        RegisterProgram(DefaultPrograms.CharacterPosition);
        RegisterProgram(DefaultPrograms.ListAvailableCommands);
        RegisterProgram(DefaultPrograms.Entities);
        RegisterProgram(DefaultPrograms.Components);
        RegisterProgram(DefaultPrograms.Spawn);
        RegisterProgram(DefaultPrograms.Singletons);
        RegisterProgram(DefaultPrograms.Exec);
        RegisterProgram(DefaultPrograms.SubBehaviour);
        RegisterProgram(DefaultPrograms.Level);
        
        Task.Factory.StartNew(Runner,TaskCreationOptions.LongRunning);
    }
    public void RunSequence(string[] sequence)
    {
        Task.Run(()=>{

        });
    }

    public void Run(string input)
    {
        if(string.IsNullOrWhiteSpace(input))
            return;

        var instructions = input.Split(';');

        try
        {

            foreach (var instruction in instructions)
            {



                ParseInstruction(instruction, out string program, out string[] parameters, out string envVar);

                RuntimeInternalLogger.Instance.Information(instruction);



                if(program.StartsWith('$'))
                {
                    _variables.TryGetValue(program, out string variable);
                    RuntimeInternalLogger.Instance.Information(variable);
                    return;
                }

                if (_commands.TryGetValue(program, out Program programInstance))
                {
                    _commandQueue.Enqueue((programInstance,parameters,envVar));
                }
                else
                {
                    RuntimeInternalLogger.Instance.Information($"Command \"{input}\" not found.");
                }
            }
        }
        catch (InvalidParameterException)
        {
            RuntimeInternalLogger.Instance.Information("Invalid parameters.");
        }
        catch(ParameterNotFoundException)
        {
            //TODO : Implement suggestions
            RuntimeInternalLogger.Instance.Information($"Not Enough parameters");
        }
        catch(Exception e)
        {
            RuntimeInternalLogger.Instance.Critical($"Error running '{input}' command.");
            RuntimeInternalLogger.Instance.Debug(e.Message);
            throw;
        }
    }



    public string[] Hint(string start)
    {
        return _commands.Keys.Where(c => c.StartsWith(start)).ToArray();
    }

    public void RegisterProgram ( Program command)
    {
        _commands.Add(command.Name,command);
    }
    
    void Runner()
    {
        while(true)
        {
            var dequeued = _commandQueue.TryDequeue(out (Program, string[],string) command);

            
            if(dequeued)
            {

                command.Item2 = command.Item2.Select(p => _variables.ContainsKey(p) ? _variables[p] : p).ToArray();
                _variables[command.Item3] = command.Item1.Run(command.Item2);
            }
            
        }
    }

    void ParseInstruction(string instruction, out string program, out string[] parameters, out string envVar)
    {
        program = "";
        envVar = "$";

        instruction = instruction.Trim();
        var parsed = instruction.Split(new char[]{' ','='},StringSplitOptions.RemoveEmptyEntries);

        List<string> commandParameters = new();

        if(!parsed[0].StartsWith('$'))
        {
            program = parsed[0];
            parameters = parsed.Length > 1 ?  parsed[1..] : Array.Empty<string>();
          
            return;
        }

        if(!instruction.Contains('='))
        {
            if(parsed.Length > 1)
                throw new Exception("Wrong format for variable assignment.");

            program = parsed[0];
            parameters = Array.Empty<string>();
     
            return;
        }

        envVar = parsed[0];
        program = parsed[1];
        parameters = parsed.Length > 2 ? parsed[2..] : Array.Empty<string>();

    }
}
