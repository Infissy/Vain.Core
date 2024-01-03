
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Vain.Core;
using Vain.Log.Visualizer;
using Vain.Singleton;

namespace Vain.Log;


public class RuntimeInternalLogger : ILogger
{


    readonly Dictionary<IOutput, LogLevel> _outputs = new();
    static RuntimeInternalLogger _instance = null;

    //? Static private property? Not the best looking solution
    static TimeSpan Timestamp => DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();


    public static RuntimeInternalLogger Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new RuntimeInternalLogger();

                #if DEBUG
                _instance.RegisterOutput(new ProxyGodotConsole(), LogLevel.Critical | LogLevel.Important | LogLevel.Warnings | LogLevel.Information | LogLevel.Debug | LogLevel.Command);
                #endif
            }
            return _instance;
        }
    }


    public void RegisterOutput(IOutput output, LogLevel level)
    {
        _outputs.Add(output, level);
    }



    public void Critical(string message)
    {
        var formattedMessage = new List<FormattedMessage>
        {
            GetFormattedTimestamp()
        };

        formattedMessage.AddRange(FormatMessage(message,LogLevel.Critical));
        OutputMessage(formattedMessage,LogLevel.Critical);

    }
    public void Warning(string message)
    {
        var formattedMessage  = new List<FormattedMessage>
        {
            GetFormattedTimestamp()
        };

        formattedMessage.AddRange(FormatMessage(message,LogLevel.Warnings));
        OutputMessage(formattedMessage,LogLevel.Warnings);

    }
    public void Debug(string message)
    {

        #if DEBUG
        var formattedMessage  = new List<FormattedMessage>
        {
            GetFormattedTimestamp()
        };

        formattedMessage.AddRange(FormatMessage(message,LogLevel.Debug));
        OutputMessage(formattedMessage,LogLevel.Debug);

        #endif
    }

    public void Important(string message)
    {
        var formattedMessage  = new List<FormattedMessage>
        {
            GetFormattedTimestamp()
        };


        formattedMessage.AddRange(FormatMessage(message,LogLevel.Important));
        OutputMessage(formattedMessage,LogLevel.Important);

    }


    public void Information(string message)
    {
        var formattedMessage  = new List<FormattedMessage>
        {
            GetFormattedTimestamp()
        };

        formattedMessage.AddRange(FormatMessage(message,LogLevel.Information));
        OutputMessage(formattedMessage,LogLevel.Information);

    }

    public void Command(string message, string parameters = "", bool showTimestamp = true)
    {
        var formattedMessage  = new List<FormattedMessage>();
        if (showTimestamp)
            formattedMessage.Add(GetFormattedTimestamp());

        formattedMessage.AddRange( FormatMessage(message,LogLevel.Command));

        formattedMessage.AddRange(FormatMessage(parameters, LogLevel.Command, OutputFormat.Italics));
        OutputMessage(formattedMessage,LogLevel.Command);

    }


    //FIXME: Runtime has to work only in debug builds, or choose if some information is accessible to the user, that can improve performance also
    public void Runtime(string label,string message, string parameters="")
    {

        //TODO: parameters will define formatting
        SingletonManager.GetSingleton<DebugOverlay>(SingletonManager.Singletons.UI.DEBUG_OVERLAY).Reference.Log(label,message);
    }


    public RuntimeInternalContextLogger SetContext(Node context){

        //TODO: Temporarely set script as identifier, later on should be implemented with ids 
        var contextLogger = new RuntimeInternalContextLogger(context.Name , "Script");
        foreach (var output in _outputs)
            contextLogger.RegisterOutput(output.Key,output.Value);

        return contextLogger;

    }


    protected void OutputMessage ( List<FormattedMessage> messages, LogLevel level)

    {
        //TODO: Optimize
        foreach (var output in _outputs)
        {

            if((output.Value & level) == 0)
                continue;


            if(output.Key is IFormattedOutput formattedOutput)
            {
                formattedOutput.Write(messages.ToArray());
                continue;
            }


            var cleanMessage = "";
            foreach (var message in messages.Select(m => m.Message))
            {
                cleanMessage +=  $" {message}";
            }
            output.Key.Write(cleanMessage);

        }
    }

    static protected FormattedMessage GetFormattedTimestamp ()
    {
        return new FormattedMessage($"{Timestamp.Minutes:D3}:{Timestamp.Seconds:D2}:{Timestamp.Milliseconds:D3}",  OutputFormat.Grey | OutputFormat.Italics);
    }

    static protected List<FormattedMessage> FormatMessage(string message, LogLevel level, OutputFormat format = OutputFormat.Default)
    {
        var formattedMessage = new List<FormattedMessage>();

        switch (level)
        {
            case LogLevel.Critical:
                formattedMessage.Add( new FormattedMessage($"Critical | {message}"  , format | OutputFormat.Red | OutputFormat.Bold));
                break;
            case LogLevel.Warnings:
                formattedMessage.Add( new FormattedMessage("Warning |" ,  format | OutputFormat.Red | OutputFormat.Bold));
                formattedMessage.Add(  new FormattedMessage( message ,  format | OutputFormat.Red));
                break;
            case LogLevel.Important:
                formattedMessage.Add( new FormattedMessage("Important | ", format | OutputFormat.Bold | OutputFormat.Blue));
                formattedMessage.Add(new FormattedMessage(   message, format | OutputFormat.Blue | OutputFormat.Green));
                break;
            case LogLevel.Debug:
                formattedMessage.Add(new FormattedMessage( "Debug | ", format | OutputFormat.Blue));
                formattedMessage.Add( new FormattedMessage ( message , format | OutputFormat.Italics | OutputFormat.Blue));
                break;
            case LogLevel.Information:
                formattedMessage.Add( new FormattedMessage(message, format | OutputFormat.Grey | OutputFormat.Bold));
                break;

            case LogLevel.Command:
                formattedMessage.Add( new FormattedMessage(message ,format |  OutputFormat.Default));
                break;

            default:
                formattedMessage.Add( new FormattedMessage(message, format | OutputFormat.Italics));
                break;
        }


        return formattedMessage;

    }


}


public class RuntimeInternalContextLogger : RuntimeInternalLogger
{

    readonly string _nodename;
    readonly string _identifier;


    public RuntimeInternalContextLogger(string nodeName, string identifier)
    {
        _nodename = nodeName;
        _identifier = identifier;

    }

    public new void Critical(string message)
    {
        ConstructMessage(message, LogLevel.Critical);
    }
    public new void Warning(string message)
    {
        ConstructMessage(message, LogLevel.Warnings);
    }
    public new void Debug(string message)
    {
        ConstructMessage(message, LogLevel.Debug);
    }
    public new void Information(string message)
    {
        ConstructMessage(message, LogLevel.Information);
    }
    public new void Important(string message)
    {
        ConstructMessage(message, LogLevel.Important);
    }


    void ConstructMessage(string message, LogLevel level)
    {
        var formattedMessage = FormatMessage(message,level);

        var injectedMessage = InjectMetadata(formattedMessage);

        OutputMessage(injectedMessage,level);

    }


    List<FormattedMessage> InjectMetadata(List<FormattedMessage> message)
    {
        message.Add(
            new FormattedMessage
            {
                Message = $"{_nodename}|{_identifier}",
                Format = OutputFormat.Default
            }
        );

        return message;

    }

}


public interface IOutput
{
    void Write(string output);
}

[Flags]
public enum LogLevel{
    Default = 0,
    Critical = 1,
    Important = 2,
    Warnings = 4,
    Information = 8,
    Debug = 16,
    Command = 32,
}




