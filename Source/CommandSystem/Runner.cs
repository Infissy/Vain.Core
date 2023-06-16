using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Vain.Log;
namespace Vain.CommandSystem

{

    
    ///<summary>
    /// Main Class that handles command execution
    ///</summary>
    public partial class Runner 
    {
        static Runner _instance = new Runner();
        
        public static Runner Instance 
        {
            get => _instance;
        }
        public Command[] Commands {get => _commands.Values.ToArray();}
    
        public Dictionary<string, Command> _commands = new Dictionary<string, Command>();


        public Runner()
        {
            RegisterCommand(DefaultCommands.Print);
            /*
            RegisterCommand(DefaultCommands.PlayerPos);
            RegisterCommand(DefaultCommands.EntityPos);
            */

            RegisterCommand(DefaultCommands.ListAvailableCommands);
                
            /*

            RegisterCommand(DefaultCommands.Entities);
            RegisterCommand(DefaultCommands.Components);

            */

        }


       
        public void Run(string command, params string[] parameters)
        {

            try
            {
                Logger.Command(command, string.Join(" ", parameters));
                Command commandInstance;
                if(_commands.TryGetValue(command,out commandInstance))
                    commandInstance.CommandFunction(parameters);
                else
                    Logger.Information($"Command \"{command}\" not found.");


                
            }
            catch (InvalidParameterException)
            {
                
                Logger.Information("Invalid parameters.");
                
            }
            catch(ParameterNotFoundException)
            {
                //TODO : Implement suggestions
                Logger.Information($"Not Enough parameters");
            }
        
        
        }




        public string[] Hint(string start)
        {
            return _commands.Keys.Where(c => c.StartsWith(start)).ToArray();
        }

        public void RegisterCommand (string name, Command.Function function)
        {


        }

        public void RegisterCommand ( Command command)
        {
            _commands.Add(command.CommandName,command);
        }
        

    }






}