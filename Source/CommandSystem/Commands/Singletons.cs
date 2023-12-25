using Vain.Log;
using Vain.Singleton;

namespace Vain.CLI;

public static partial class DefaultPrograms
{
    public static readonly Program Singletons = new()
    {
        Name = "singletons",

        Commands =
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
                    RuntimeInternalLogger.Instance.Information(singletonList);
                }
            )
        }
    };
}