using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Godot;
using Vain.Log;
namespace Vain.CLI

{

    
    ///<summary>
    /// Main Class that handles command execution
    ///</summary>
    internal class CommandRunner 
    {


      
        readonly Dictionary<string, Program> _commands = new();

        readonly Dictionary<string,string> _variables = new();


        
        public Program[] Commands  => _commands.Values.ToArray();
    





        static CommandRunner _instance = new();
        public static CommandRunner Instance => _instance;

        public CommandRunner()
        {
            RegisterCommand(DefaultPrograms.CharacterPosition);
            RegisterCommand(DefaultPrograms.ListAvailableCommands);
            RegisterCommand(DefaultPrograms.Entities);
            RegisterCommand(DefaultPrograms.Components);
            RegisterCommand(DefaultPrograms.Spawn);
            RegisterCommand(DefaultPrograms.SingletonList);

        }


       
        public void Run(string command)
        {

           
            var parsed = command.Split(' ');
            

            try
            {

                var outputVar = "$";
                List<string> commandParameters = new();

                if(parsed[0].StartsWith('$'))
                {
                    if(!parsed[0].Contains('=') || !parsed[1].StartsWith('$'))
                        throw new Exception("Wrong format for variable assignment.");


                    if(parsed[0].Contains('='))
                    {
                        var split = parsed[0].Split('=');

                    }
                }

                Logger.Command(command, string.Join(" ", parameters));
                Program commandInstance;
                if(_commands.TryGetValue(command,out commandInstance))
                    _variables[].commandInstance.Run(parameters);
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
            catch(Exception e)
            {
                Logger.Critical($"Error running '{command}' command.");
                Logger.Debug(e.Message);
                throw;
            }
        
        
        }




        public string[] Hint(string start)
        {
            return _commands.Keys.Where(c => c.StartsWith(start)).ToArray();
        }

        public void RegisterCommand (string name, Delegate function)
        {


        }

        public void RegisterCommand ( Program command)
        {
            _commands.Add(command.ProgramName,command);
        }
        

    }






}