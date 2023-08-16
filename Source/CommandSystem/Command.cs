using System;
using System.Linq;
using System.Collections.Generic;


using Vain.Core;


using Vain.Log;
using Vain.Singleton;
using Vain.UI;
using Godot;

//Not working


namespace Vain.CommandSystem
{

    public class Command
    {



        
        
        public string CommandName {get;set;} = "default";

        public string CommandParameters {get;set;} = "";

        public List<Delegate> CommandFunction {get;set;} = new List<Delegate>();
        

        

        public void Run(object[] parameters)
        {
            var types = parameters.Select(p => p.GetType());


            var command = CommandFunction.Where(d =>  d.Method.GetParameters().Select(p => p.ParameterType).SequenceEqual(types)).FirstOrDefault();

            if(command == null)
                throw new InvalidParameterException();
            

            command.DynamicInvoke(parameters);
        }

        

        
    }

    public class CommandFunctionOverloadException : System.Exception
    {

        public CommandFunctionOverloadException() : base("Command function overload not permitted.") { }
        
    }

    public static class DefaultCommands
    {

        
        
        public static Command CharacterPosition = new Command
        {
            CommandName  = "characterpos",


            CommandFunction = 
            {
                (int index) => {
                
                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == index).FirstOrDefault();
                    if(entity == null)
                    {
                        Logger.Information($"No entity found with given ID ({index})");
                        return;
                    }
                    Logger.Information(entity.Position.ToString());
                },


                (string code) => {
                    

                    if(code == "player")
                    {
                        var entity = SingletonManager.GetCharacterSingleton(SingletonManager.Singletons.PLAYER);
                        Logger.Information(entity.Reference.Position.ToString());
                    }
                }
            
            }
        
        
        };
        

        public static Command ListAvailableCommands = new Command
        {
            CommandName  = "?",
            
            CommandFunction = 
            {
                
                () => 
                    {
                        var runner = CommandRunner.Instance;
                        foreach (var command in CommandRunner.Instance.Commands)
                        {
                            Logger.Command(command.CommandName,command.CommandParameters, false);
                        }
                    }

            }

        };

        
        public static Command Entities = new Command
        {
            CommandName  = "entities",
            CommandFunction = 
            {
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
            }

        };

        
        public static Command Components = new Command
        {
            CommandName  = "component",

            CommandParameters = "entity id : number list/add/remove",
            
            
            CommandFunction =  
            {
                (int id) => {

                   
                    
                    
                    
                    var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Entities.Where(e => e.RuntimeID == id).First();

                    var msg = "\n";
                    

                    
                    foreach (var component in (entity as Character).GetComponents())
                    {
                        msg += $"   {component.GetType().Name } \n";
                    

                    }
                    Logger.Information(msg);
                }


            }
        };


        public static Command Spawn = new Command
        {

            CommandName = "spawn",
            

            CommandParameters = "entity : text",
            

            CommandFunction = 
            {
                (string entity) => 
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
                    
                },

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
            
            }

        };
        public static Command SingletonList = new Command
        {

            CommandName = "singletons",
            

            

            CommandFunction = 
            {
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
            }
        };  




        

    }


}