
using System.Threading.Tasks;
using Godot;
using Vain.Core;
using Vain.HubSystem;
using Vain.HubSystem.GameEvent;
using Vain.LevelSystem;
using Vain.Log;
using Vain.Singleton;
using static Vain.HubSystem.GameEvent.GameEvents;
using static Vain.HubSystem.GameEvent.GameEvents.Entity;

namespace Vain.CLI;
public static partial class DefaultPrograms
{
    class LevelLoadedProxy : IListener<LevelChangedEvent,LevelNameEventArgs>
    {
        TaskCompletionSource<bool> _task;
        public LevelLoadedProxy(TaskCompletionSource<bool> task)
        {
            _task = task;
            Hub.Instance.Subscribe(this);
        }
        public void HandleEvent(LevelNameEventArgs args)
        {
            _task.SetResult(true);
        }
        ~LevelLoadedProxy()
        {
            Hub.Instance.Unsubscribe(this);
        }
    }

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
                    var levels = GameRegistry.Instance.LevelIndex.IndexedEntities.Keys;

                    var outputLevels = "\nAvailable Levels:\n";

                    foreach (var level in levels)
                    {
                        outputLevels += $"      {level}\n";
                    }
                    outputLevels += "\n";
                    RuntimeInternalLogger.Instance.Information(outputLevels);
                    
                    
                    return "";
                }
            ),
            new Command
            (
                "load ?:s",
                async (string key) => 
                {
                    

                    var waitingTask = new TaskCompletionSource<bool>();
                    var proxy = new LevelLoadedProxy(waitingTask);
                    Hub.Instance.Emit<LevelChangeRequestEvent,LevelNameEventArgs>(new LevelNameEventArgs{ LevelName = key});
                    

                    

                    
                    await waitingTask.Task;

                    RuntimeInternalLogger.Instance.Information($"Loaded level {key}");

                    return "";
                } 
                    
            ),
        }
    };
}