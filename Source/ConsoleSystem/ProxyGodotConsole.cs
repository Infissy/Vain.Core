using System;
using Godot;
namespace Vain.Log;

internal class ProxyGodotConsole : IFormattedOutput
{
    public void Write(FormattedMessage[] formattedOutput)
    {
        foreach (var line in formattedOutput)
        {
            var output = line.Message;
            if((line.Format | OutputFormat.Bold) == OutputFormat.Bold)
                output = $"[b]{output}[/b]";
            if((line.Format | OutputFormat.Italics) == OutputFormat.Italics)
                output = $"[i]{output}[/i]";
            if((line.Format | OutputFormat.Blue) == OutputFormat.Blue)
                output = $"[color=#0000ff]{output}[/color]";
            if((line.Format | OutputFormat.Red) == OutputFormat.Red)
                output = $"[color=#ff0000]{output}[/color]";
            if((line.Format | OutputFormat.Green) == OutputFormat.Green)
                output = $"[color=#00ff00]{output}[/color]";
            if((line.Format | OutputFormat.Grey) == OutputFormat.Grey)
                output = $"[color=#aaaaaa]{output}[/color]";

            GD.PrintRich(output);
        }
    }

    public void Write(string output)
    {
        GD.Print(output);
    }
}
