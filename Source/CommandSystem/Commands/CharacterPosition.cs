using System.Linq;
using Vain.Core;
using Vain.Log;
using Vain.Singleton;

namespace Vain.CLI;



public static partial class DefaultPrograms
{
    //Position is a character attribute, maybe merge all attributes in a single command instead of having a separate command for each attribute?
    public static readonly Program CharacterPosition = new()
    {
        Name  = "characterpos",

        Commands =
        {
            new Command
            (
                "?:n",
                (int index) => {
                var entity = SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference.Characters.FirstOrDefault(e => e.RuntimeID == index);
                if(entity == null)
                {
                    RuntimeInternalLogger.Instance.Information($"No entity found with given ID ({index})");
                    return;
                }
                RuntimeInternalLogger.Instance.Information(entity.Position.ToString());
            }),

            new Command
            (
                "?:s",
                (string code) => {
                    if(code == "player")
                    {
                        var entity = SingletonManager.GetCharacterSingleton(SingletonManager.Singletons.PLAYER);
                        RuntimeInternalLogger.Instance.Information(entity.Reference.Position.ToString());
                    }
                }
            )
        }
    };
}