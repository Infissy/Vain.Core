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
            RegisterProgram(DefaultPrograms.CharacterPosition);
            RegisterProgram(DefaultPrograms.ListAvailableCommands);
            RegisterProgram(DefaultPrograms.Entities);
            RegisterProgram(DefaultPrograms.Components);
            RegisterProgram(DefaultPrograms.Spawn);
            RegisterProgram(DefaultPrograms.SingletonList);

        }


       
        public void Run(string input)
        {

            var instructions = input.Split(';');
            
     


            
            try
            {


                foreach (var instruction in instructions)
                {
                    

                    string program;
                    string[] parameters;
                    string envVar;
                    
                    parseInstruction(instruction,out program, out parameters,out envVar);
                    
                    Logger.Information(instruction);



                    Program programInstance;
                    if(program.StartsWith('$'))
                    {
                        var variable = "";
                        _variables.TryGetValue(program,out variable);

                        Logger.Information(variable);
                        return;
                    }


                    if(_commands.TryGetValue(program,out programInstance))
                         _variables[envVar] = programInstance.Run(parameters);
                    else
                        Logger.Information($"Command \"{input}\" not found.");
        

                }
             
         
                
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
                Logger.Critical($"Error running '{input}' command.");
                Logger.Debug(e.Message);
                throw;
            }
        
        
        }

        


        public string[] Hint(string start)
        {
            return _commands.Keys.Where(c => c.StartsWith(start)).ToArray();
        }

        public void RegisterProgram ( Program command)
        {
            _commands.Add(command.ProgramName,command);
        }




        void parseInstruction(string instruction, out string program, out string[] parameters, out string envVar)
        {
            
            envVar = "$";


            
            instruction = instruction.Trim();
            var parsed = instruction.Split(' ','=');


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

            parameters.Select(p => p.StartsWith('$') ? _variables[p] : p);
          
        }
            

    }






}