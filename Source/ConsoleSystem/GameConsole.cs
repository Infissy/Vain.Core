using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Vain.CLI;
using Vain.Log;


namespace Vain.Console;


///<summary>
/// GameConsole is the main  <see cref="P:Godot.Node"/> that contains the logic for the output.
///</summary>
public partial class GameConsole : Node, IFormattedOutput
{
    [Signal]
    public delegate void OnUpdateEventHandler();

    LineEdit _inputBox;
    bool _historyMode = false;

    internal ConsoleBuffer History {get;set;} = new();

    public List<string> _runtimeBuffer = new();

    public List<string> BufferedMessages => _runtimeBuffer;

    public  override void _Ready()
    {
        base._Ready();

        //TODO : Change format, specially for build releases where debug stuff should not be showed
        RuntimeInternalLogger.Instance.RegisterOutput(this, LogLevel.Critical | LogLevel.Important | LogLevel.Warnings | LogLevel.Information | LogLevel.Command
            #if DEBUG
            | LogLevel.Debug
            #endif
        );

        var button = GetNode("VBoxContainer/HBoxContainer/Button") as Button;
        _inputBox = GetNode("VBoxContainer/HBoxContainer/LineEdit") as LineEdit;

        button.Pressed += CommandSent;
        _inputBox.TextSubmitted += ConsoleButtonPressed;
        _inputBox.TextChanged += (string text) =>
            {
                _historyMode = (text == History.Current());
                History.ResetPointer();
            };

        History.LoadHistory();

        //!FIXME: starting script is dependent on gameconsole. Implement a separate service that can call this function
      
        var timer = GetTree().CreateTimer(1);

        timer.Connect("timeout", new Callable(this,MethodName.ExecAutoExec));
    }

    public override void _Input(InputEvent @input)
    {
        if(_inputBox.Text.Length == 0 || _historyMode)
        {
            //FIXME: Remove static action reference, move it all into one place
            if (@input.IsActionPressed("ui_up"))
            {
                _inputBox.Text = History.Back();
                    CallDeferred(MethodName.CursorToEnd);
                _historyMode = true;
            }

            if(@input.IsActionPressed("ui_down"))
            {
                _inputBox.Text = History.Forward();
                CallDeferred(MethodName.CursorToEnd);

                _historyMode = true;
            }
        }
    }

    public void CommandSent()
    {
        ConsoleButtonPressed(_inputBox.Text);
    }

    public void ConsoleButtonPressed(string text){
        if(text.Length == 0)
            return;

        CommandRunner.Instance.Run(text);
        History.Add(text);
        _inputBox.Clear();


    }

    public void Write(string output)
    {
        _runtimeBuffer.Add(output);

        EmitSignal(SignalName.OnUpdate);

        _runtimeBuffer.Clear();

    }

    public void Write(FormattedMessage[] formattedOutput)
    {

        var message ="";
        foreach (var messageSegment in formattedOutput)
        {
            message +=  $" {ParseMessage(messageSegment.Message,messageSegment.Format)}";
        }



        _runtimeBuffer.Add(message);
        CallDeferred(GameConsole.MethodName.EmitSignal, SignalName.OnUpdate);
        
    }


    static string ParseMessage(string message, OutputFormat format){

        string parsedMessage = message;

        var color = Colors.Black;


        if((format & OutputFormat.Bold) == OutputFormat.Bold)
            parsedMessage = $"[b]{parsedMessage}[/b]";
        if((format & OutputFormat.Italics) == OutputFormat.Italics)
            parsedMessage = $"[i]{parsedMessage}[/i]";

        if((format & OutputFormat.Red) == OutputFormat.Red)
            color.R = 255;
        if((format & OutputFormat.Blue) == OutputFormat.Blue)
            color.G = 255;
        if((format & OutputFormat.Green) == OutputFormat.Green)
            color.B = 255;
        if((format & OutputFormat.Grey) == OutputFormat.Grey)
            color = Colors.DarkSlateGray;



        if((format & FormatMasks.ColorMask ) == 0)
            color = Colors.White;

        return $"[color=#{color.ToHtml(false)}]{parsedMessage}[/color]";
    }


    void CursorToEnd()
    {
        _inputBox.CaretColumn = _inputBox.Text.Length;
    }

    
    void ExecAutoExec()
    {
        CommandRunner.Instance.Run("exec autoexec");
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        History.SaveHistory();
    }

}
