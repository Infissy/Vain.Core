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
                        Logger.GlobalLogger.Information($"No entity found with given ID ({index})");
                        return;
                    }
                    Logger.GlobalLogger.Information(entity.Position.ToString());
                }),


                new Command
                (
                    "?:s",
                    (string code) => {
                    

                        if(code == "player")
                        {
                            var entity = SingletonManager.GetCharacterSingleton(SingletonManager.Singletons.PLAYER);
                            Logger.GlobalLogger.Information(entity.Reference.Position.ToString());
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
                            Logger.GlobalLogger.Command(command.ProgramName,command.ProgramDescription, false);
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
                            Logger.GlobalLogger.Information("No Level Manager in the scene.");
                        }   
                            


                        foreach (var entity in SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Entities)
                        {
                            msg += $"   {entity.RuntimeID}  | {(entity as Node).Name}  \n";
                        }

                        Logger.GlobalLogger.Information(msg);



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
                            
                        var components = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.ComponentIndex.IndexedEntities.Keys;
                    
                        var outputComponents = "\nAvailable Components:\n";

                        foreach (var component in components)
                        {
                            outputComponents += $"      {component}\n";
                        }
                        outputComponents += "\n";
                        Logger.GlobalLogger.Information(outputComponents);
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
                        Logger.GlobalLogger.Information(msg);
                    }
                ),

                new Command 
                (
                    "add ?:n ?:s",
                    (int id, string componentName) => {

                        GodotObject componentScene;
                        var componentSuccessfulyFetched = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.ComponentIndex.IndexedEntities.TryGetValue(componentName,out componentScene);
                        if(!componentSuccessfulyFetched)
                        {
                           Logger.GlobalLogger.Warning($"No component found with name '{componentName}.'");
                           return;
                        }

                        
                        var component = (componentScene as IndexedResourceWrapper).Instantiate() as Component;

                        var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == id).First();
                        entity.AddComponent(component);
                      

                        
                        Logger.GlobalLogger.Information($"Component {componentName} successfully added.");
                    }
                )

            }
        };

        public static readonly Program SubBehaviour = new()
        {
            ProgramName  = "sub_behaviour",

            ProgramDescription = "character id : number list/add/remove",
            
            
            CommandFunction =  
            {

                new Command
                (
                    "list",
                    () =>
                    {
                            
                        var behaviours = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.BehaviourIndex.IndexedEntities.Keys;
                    
                        var outputBehaviours = "\nAvailable Behaviour:\n";

                        foreach (var behaviour in behaviours)
                        {
                            outputBehaviours += $"      {behaviour}\n";
                        }
                        outputBehaviours += "\n";
                        Logger.GlobalLogger.Information(outputBehaviours);
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
                        Logger.GlobalLogger.Information(msg);
                    }
                ),

                new Command 
                (
                    "add ?:n ?:s",
                    (int id, string behaviourName) => {

                        GodotObject behaviourScene;
                        var componentSuccessfulyFetched = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.BehaviourIndex.IndexedEntities.TryGetValue(behaviourName,out behaviourScene);
                        if(!componentSuccessfulyFetched)
                        {

                           Logger.GlobalLogger.Warning($"Behaviour with name {behaviourName} was not found.");
                           return;
                        }

                        var behaviour = (behaviourScene as IndexedResourceWrapper).Instantiate() as SubBehaviour;

                        var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == id).First();
                        entity.GetComponent<CharacterBehaviourComponent>().AddChild(behaviour);
                      

                        
                        Logger.GlobalLogger.Information($"Component {behaviourName} successfully added.");
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
                        var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities;
                        


                        if(!entities.ContainsKey(entity))
                        {
                            Logger.GlobalLogger.Information($"No entity found with identifier {entity}");
                            return "";
                        }


                        
                        
                        var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities[entity];

                        var instance = (entityPrefab as PackedScene).Instantiate();
                        

                        
                        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);
                        


                        return (instance as IEntity).RuntimeID.ToString();
                    }
                ),
                new Command
                (
                    "?:s ?:f ?:f ?:f",
                    (string entity,float x, float y) => 
                    {


                        


                        var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities;
                        


                        if(!entities.ContainsKey(entity))
                        {
                            Logger.GlobalLogger.Information($"No entity found with identifier {entity}");
                            return "";
                        }


                        
                        
                        var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities[entity];

                        var instance = (entityPrefab as PackedScene).Instantiate();
                        

                        
                        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);
                        
                    
                        (instance as Node2D).GlobalPosition = new Vector2(x,y);


                        return (instance as IEntity).RuntimeID.ToString();
                        
                    }
                
                ),
                new Command
                (
                    "?:s ?:s",
                    (string entity,string spawnPointTag) => 
                    {


                        


                        var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities;
                        


                        if(!entities.ContainsKey(entity))
                        {
                            Logger.GlobalLogger.Information($"No entity found with identifier {entity}");
                            return "";
                        }
                        

                        var spawnPoint = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.SpawnPoints.GetValueOrDefault(spawnPointTag);
                
                        if(spawnPoint == null)
                        {
                            Logger.GlobalLogger.Information($"No spawn point found with tag {spawnPointTag}");
                            return "";
                        }
                        
                        
                        var entityPrefab = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.IndexedEntities[entity];

                        var instance = (entityPrefab as PackedScene).Instantiate();
                        

                        
                        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild(instance);
                        
                    
                        (instance as Node2D).GlobalPosition = spawnPoint.GlobalPosition;


                        return (instance as IEntity).RuntimeID.ToString();
                        
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
                        Logger.GlobalLogger.Information(singletonList);
                        
                    }
                )
            }
        };  


        public static readonly Program Exec = new()
        {

            ProgramName = "exec",
            

            

            CommandFunction = 
            {
                new Command 
                (
                    "?:s",
                    (string scriptName) => 
                    {
                        var script = ScriptLoader.LoadScript(scriptName.Contains(".cfg") ? scriptName : scriptName + ".cfg");
                        
                        if(script == null)
                        {
                            Logger.GlobalLogger.Warning($"Script {scriptName} does not exist.");
                            return;
                        }
                        script.Run();
                    }
                )
            }
        };  


    
          public static readonly Program Level = new()
        {
            ProgramName  = "level",

            ProgramDescription = "list / load key:string",
            
            
            CommandFunction =  
            {

                new Command
                (
                    "list",
                    () =>
                    {
                            
                        var levels = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.LevelIndex.IndexedEntities.Keys;
                    
                        var outputLevels = "\nAvailable Levels:\n";

                        foreach (var level in levels)
                        {
                            outputLevels += $"      {level}\n";
                        }
                        outputLevels += "\n";
                        Logger.GlobalLogger.Information(outputLevels);
                    }
                ),
                new Command 
                (
                    "load ?:s",
                    (string key) => {
                        
                        SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference?.LoadLevel(key);
                    }
                ),
            }
        };

    }


}