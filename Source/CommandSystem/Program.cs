using System;
using System.Linq;
using System.Collections.Generic;


using Vain.Core;


using Vain.Log;
using Vain.Singleton;
using Vain.UI;
using Godot;
using System.Data;
using Vain.Core.ComponentSystem;

//Not working


namespace Vain.CLI
{

    public class Program
    {



        
        
        public string ProgramName {get;set;} = "default";

        public string ProgramDescription {get;set;} = "";

        public List<Command> CommandFunction {get;set;} = new List<Command>();
        

        

        public string Run(string[] parameters)
        {
            

            object[] parsedParameters = null;
            Command workingCommand = null;
            foreach (var command in CommandFunction)
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

    public class CommandFunctionOverloadException : System.Exception
    {

        public CommandFunctionOverloadException() : base("Command function overload not permitted.") { }
        
    }

    public static class DefaultPrograms
    {

        
        
        public static readonly Program CharacterPosition = new()
        {
            ProgramName  = "characterpos",


            CommandFunction = 
            {
                new Command
                (
                    "?:n",
                    (int index) => {
                
                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == index).FirstOrDefault();
                    if(entity == null)
                    {
                        Logger.Information($"No entity found with given ID ({index})");
                        return;
                    }
                    Logger.Information(entity.Position.ToString());
                }),


                new Command
                (
                    "?:s",
                    (string code) => {
                    

                        if(code == "player")
                        {
                            var entity = SingletonManager.GetCharacterSingleton(SingletonManager.Singletons.PLAYER);
                            Logger.Information(entity.Reference.Position.ToString());
                        }
                    }
                )
            
            }
        
        
        };
        

        public static readonly Program ListAvailableCommands = new()
        {
            ProgramName  = "?",
            
            CommandFunction = 
            {

                new Command
                (
                    "",
                    ()=>{
                        
                        var runner = CommandRunner.Instance;
                        foreach (var command in CommandRunner.Instance.Commands)
                        {
                            Logger.Command(command.ProgramName,command.ProgramDescription, false);
                        }

                })
            }

        };

        
        public static readonly Program Entities = new()
        {
            ProgramName  = "entities",
            CommandFunction = 
            {
                new Command
                (
                    "list",
                    () => {

                        var msg = "\n";
                        
                        var levelManager = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference;

                        if(levelManager == null)
                        {
                            Logger.Information("No Level Manager in the scene.");
                        }   
                            


                        foreach (var entity in SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Entities)
                        {
                            msg += $"   {entity.RuntimeID}  | {(entity as Node).Name}  \n";
                        }

                        Logger.Information(msg);



                    }
                )
            }

        };

        
        public static readonly Program Components = new()
        {
            ProgramName  = "component",

            ProgramDescription = "character id : number list/add/remove",
            
            
            CommandFunction =  
            {

                new Command
                (
                    "list",
                    () =>
                    {
                            
                        var components = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.ComponentIndex.Components.Keys;
                    
                        var outputComponents = "\nAvailable Components:\n";

                        foreach (var component in components)
                        {
                            outputComponents += $"      {component}\n";
                        }
                        outputComponents += "\n";
                        Logger.Information(outputComponents);
                    }
                ),
                new Command 
                (
                    "list ?:n",
                    (int id) => {

                       
                        var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == id).First();

                        var msg = "\n";
                        

                        
                        foreach (var component in (entity as Character).GetComponents())
                        {
                            msg += $"   {component.GetType().Name } \n";
                        

                        }
                        Logger.Information(msg);
                    }
                ),

                new Command 
                (
                    "add ?:n ?:s",
                    (int id, string componentName) => {

                        GodotObject componentScene;
                        var componentSuccessfulyFetched = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.ComponentIndex.Components.TryGetValue(componentName,out componentScene);
                        if(!componentSuccessfulyFetched)
                            throw new InvalidParameterException();

                        var component = (componentScene as PackedScene).Instantiate<Component>();

                        var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == id).First();
                        entity.AddComponent(component);
                      

                        
                        Logger.Information($"Component {componentName} successfully added.");
                    }
                )

            }
        };


        public static readonly Program Spawn = new()
        {

            ProgramName = "spawn",
            

            ProgramDescription = "entity : text",
            

            CommandFunction = 
            {
                new Command
                (
                    "?:s",
                    (string entity) => 
                    {
                        var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.Entities;
                        


                        if(!entities.ContainsKey(entity))
                        {
                            Logger.Information($"No entity found with identifier {entity}");
                            return "";
                        }


                        
                        
                        var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.Entities[entity];

                        var instance = (entityPrefab as PackedScene).Instantiate();
                        

                        
                        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);
                        


                        return (instance as IEntity).RuntimeID.ToString();
                    }
                ),
                new Command
                (
                    "?:s ?:f ?:f ?:f",
                    (string entity,float x, float y, float z) => 
                    {


                        


                        var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.Entities;
                        


                        if(!entities.ContainsKey(entity))
                        {
                            Logger.Information($"No entity found with identifier {entity}");
                            return;
                        }


                        
                        
                        var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.Entities[entity];

                        var instance = (entityPrefab as PackedScene).Instantiate();
                        

                        
                        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);
                        
                    
                        (instance as Node3D).GlobalPosition = new Vector3(Convert.ToSingle(entity),Convert.ToSingle(entity),Convert.ToSingle(entity));
                        
                    }
                
                )
            
            }

        };
        public static readonly Program SingletonList = new()
        {

            ProgramName = "singletons",
            

            

            CommandFunction = 
            {
                new Command 
                (
                    "",
                    () => 
                    {

                        var singletons = SingletonManager.GetSingletonsList();
                        string singletonList = "\n";
                        foreach (var singleton in singletons)
                        {
                            singletonList += singleton + '\n';
                        }
                        Logger.Information(singletonList);
                        
                    }
                )
            }
        };  




        

    }


}