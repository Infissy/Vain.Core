
using Vain.Core;
using Vain.Log;
using Vain.Singleton;

namespace Vain.CLI;
public static partial class DefaultPrograms
{
    public static readonly Program Level = new()
    {
        Name  = "level",

        Description = "list / load key:string",
        Commands =
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
                    RuntimeInternalLogger.Instance.Information(outputLevels);
                }
            ),
            new Command
            (
                "load ?:s",
                (string key) => SingletonManager.GetSingleton<LevelManager>(SingletonManager.Singletons.LEVEL_MANAGER).Reference?.LoadLevel(key)
            ),
        }
    };
}