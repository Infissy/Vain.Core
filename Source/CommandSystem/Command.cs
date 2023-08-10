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

    public partial class Command
    {


        
        public delegate void Function(params object[] parameters);
        
        public string CommandName = "default";

        public string CommandParameters = "";

        public Function CommandFunction = (_)=>{};
        


        

        
    }

    public static class DefaultCommands
    {

        
        public static Command Print = new Command
        {

            CommandName = "print",
            

            CommandParameters = "message : text",
            

            CommandFunction = (pm) => Logger.Information($"\"{pm[0] as string}\"")
        
        };  


        public static Command PlayerPos = new Command
        {
            CommandName  = "playerpos",


            CommandFunction = (pm) => 
            {
                var player = SingletonManager.GetSingleton<Character>(SingletonManager.Singletons.PLAYER);
                
                if(player == null)
                {
                    Logger.Information("No player in the scene");
                    return;
                }

                    
                Logger.Information(player.Reference.Position.ToString());

            }

        };

        
        public static Command CharacterPosition = new Command
        {
            CommandName  = "characterpos",


            CommandFunction = (pm) => {
                
                var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.Where(e => e.RuntimeID == int.Parse(pm[0] as string)).First();

                Logger.Information(entity.Position.ToString());
            }


        };
        

        public static Command ListAvailableCommands = new Command
        {
            CommandName  = "?",
            
            CommandFunction = (_) => 

            {
                var runner = CommandRunner.Instance;
                foreach (var command in CommandRunner.Instance.Commands)
                {
                    Logger.Command(command.CommandName,command.CommandParameters, false);
                }
            }


        };

        
        public static Command Entities = new Command
        {
            CommandName  = "entities",
            CommandFunction = (pm) => {

                var msg = "\n";
                
                var levelManager = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference;

                if(levelManager == null)
                {
                    Logger.Information("No Level Manager in the scene.");
                }   
                    


                foreach (var entity in SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Entities)
                {
                    msg += $"   ({entity.RuntimeID}){(entity as Node).Name}  \n";
                }

                Logger.Information(msg);



            }


        };

        
        public static Command Components = new Command
        {
            CommandName  = "components",

            CommandParameters = "entity id : number",
            
            
            CommandFunction = (pm) => {

                var id = -1;
                
                try
                {
                    id = int.Parse(pm[0] as string);
                    
                }
                catch (IndexOutOfRangeException)
                {
                    
                    throw new ParameterNotFoundException();
                }
                catch(InvalidCastException)
                {
                    throw new InvalidParameterException();
                }
                
                
                
                var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Entities.Where(e => e.RuntimeID == id).First();

                var msg = "\n";
                

                
                foreach (var component in (entity as Character).GetComponents())
                {
                    msg += $"   {component.GetType().Name } \n";
                

                }
                Logger.Information(msg);
            }



        };


        public static Command Spawn = new Command
        {

            CommandName = "spawn",
            

            CommandParameters = "entity : text",
            

            CommandFunction = (pm) => 
            {

                var entities = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.Entities;

                if(!entities.ContainsKey(pm[0].ToString()))
                {
                    Logger.Information($"No entity found with identifier {pm[0].ToString()}");
                    return;
                }


                
                var entity = SingletonManager.GetSingleton<GameRegistry>(SingletonManager.Singletons.GAME_REGISTRY).Reference.EntityIndex.Entities[pm[0].ToString()];


                SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.AddChild((entity as PackedScene).Instantiate());
                
            }
        
        };  

        
        public static Command SingletonList = new Command
        {

            CommandName = "singletons",
            

            

            CommandFunction = (pm) => 
            {

                var singletons = SingletonManager.GetSingletonsList();
                string singletonList = "\n";
                foreach (var singleton in singletons)
                {
                    singletonList += singleton + '\n';
                }
                Logger.Information(singletonList);
                
            }
        
        };  




        

    }


}