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


            CommandFunction = (pm) => Logger.Information(SingletonManager.GetSingleton<Player>().CurrentCharacter.Position.ToString())


        };


        
        public static Command CharacterPosition = new Command
        {
            CommandName  = "entitypos",


            CommandFunction = (pm) => {
                
                var entity = SingletonManager.GetSingleton<LevelManager>().Characters.Where(e => e.RuntimeID == int.Parse(pm[0] as string)).First();

                Logger.Information(entity.Position.ToString());
            }


        };
        


         public static Command ListAvailableCommands = new Command
        {
            CommandName  = "?",
            
            CommandFunction = (_) => 

            {
                var runner = Runner.Instance;
                foreach (var command in Runner.Instance.Commands)
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
                


                foreach (var entity in SingletonManager.GetSingleton<LevelManager>().Entities)
                {
                    msg += $"   ({entity.RuntimeID}){(entity as Node).Name}  \n";
                

                }

                Logger.Information(msg);



            }


        };

            


        /*
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
                
                
                
                var entity = Character.Where(e => e.ID == id).First();

                var msg = "\n";
                

                
                foreach (var component in entity.ComponentContainer.Components)
                {
                    msg += $"   {component.GetType().Name } \n";
                

                }
                Logger.Information(msg);
            }



        };
                */
        

    }


}