namespace Vain.CLI;

public class Script
{
    public string[] TextScript { get; }
    public Script(string text)
    {
        TextScript = text.Split("\n");
    }
    public void Run()
    {
        foreach (var line in TextScript)
        {
            CommandRunner.Instance.Run(line);
        }
    }
}
