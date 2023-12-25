
using Godot;
using Vain.Core;
using Vain.Log;
using Vain.Singleton;

namespace Vain.CLI;

public static partial class DefaultPrograms
{
     public static readonly Program Entities = new()
    {
        Name  = "entities",
        Commands = 
        {
            new Command
            (
                "list",
                () => {

                    var msg = "\n";

                    var levelManager = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference;

                    if(levelManager == null)
                    {
                        RuntimeInternalLogger.Instance.Information("No Level Manager in the scene.");
                    }   
                        


                    foreach (var entity in SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Entities)
                    {
                        msg += $"   {entity.RuntimeID}  | {(entity as Node).Name}  \n";
                    }

                    RuntimeInternalLogger.Instance.Information(msg);



                }
            )
        }

    };

}