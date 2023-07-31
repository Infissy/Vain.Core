
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Vain.Core;
using Vain.Log.Visualizer;
using Vain.Singleton;

namespace Vain.Log
{
    
    // !  At the moment the logging system is broken
    
    public partial class Logger
    {

        //? Static private property? Not the best looking solution
        static TimeSpan Timestamp{
            get
            {

            return DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();
            }
        }






        static Dictionary<IOutput, LogLevel> _outputs = new Dictionary<IOutput, LogLevel>();


        public static ContextLogger SetContext(Node context){
            
            //TODO: Temporarely set script as identifier, later on should be implemented with ids 
            var contextLogger = new ContextLogger(context.Name , "Script");

            return contextLogger;
            
          
        }


        public static void RegisterOutput(IOutput output, LogLevel level){
            _outputs.Add(output, level);
        }



        public static void Critical(string message)
        {   
             var formattedMessage  = new List<FormattedMessage>();
            formattedMessage.Add(timestamp());

            formattedMessage.AddRange(formatMessage(message,LogLevel.Critical));
            outputMessage(formattedMessage,LogLevel.Critical);

            
        }
        public static void Warning(string message)
        {     
            var formattedMessage  = new List<FormattedMessage>();
            formattedMessage.Add(timestamp());

            formattedMessage.AddRange(formatMessage(message,LogLevel.Warnings));
            outputMessage(formattedMessage,LogLevel.Warnings);

            
        }
        public static void Debug(string message)
        {
             var formattedMessage  = new List<FormattedMessage>();
            formattedMessage.Add(timestamp());

            formattedMessage.AddRange(formatMessage(message,LogLevel.Debug));
            outputMessage(formattedMessage,LogLevel.Debug);

            
        }

        public static void Important(string message)
        {
             var formattedMessage  = new List<FormattedMessage>();
            formattedMessage.Add(timestamp());

            formattedMessage.AddRange(formatMessage(message,LogLevel.Important));
            outputMessage(formattedMessage,LogLevel.Important);

            
        }


        public static void Information(string message)
        {
             var formattedMessage  = new List<FormattedMessage>();
            formattedMessage.Add(timestamp());

            formattedMessage.AddRange(formatMessage(message,LogLevel.Information));
            outputMessage(formattedMessage,LogLevel.Information);

            
        }
        

        public static void Command(string message, string parameters = "", bool showTimestamp = true)
        {
            var formattedMessage  = new List<FormattedMessage>();
            if (showTimestamp)
                formattedMessage.Add(timestamp());

            formattedMessage.AddRange( formatMessage(message,LogLevel.Command));

            formattedMessage.AddRange(formatMessage(parameters, LogLevel.Command, OutputFormat.Italics));
            outputMessage(formattedMessage,LogLevel.Command);

            
        }


        //FIXME: Runtime has to work only in debug, or choose if some information is accessible to the user, that can improve performance also
        public static void Runtime(string label,string message, string parameters="")
        {

            //TODO: parameters will define formatting
            SingletonManager.GetSingleton<DebugOverlay>().Log(label,message);
        }

        static protected void outputMessage ( List<FormattedMessage> messages, LogLevel level)
        
        {
            //TODO: Optimize
            foreach (var output in _outputs)
            {

                if((output.Value & level) != 0){

                    if(output.Key is IFormattedOutput){



                        ((IFormattedOutput)output.Key).Write(messages.ToArray());


                    }
                    else
                    {

                        var cleanMessage = "";
                        foreach (var message in messages.Select(m => m.Message))
                        {
                            cleanMessage +=  $" {message}";
                        }
                        output.Key.Write(cleanMessage);

                    }
                }




                


            }   
        }

        static protected FormattedMessage timestamp ()
        {
            return new FormattedMessage($"{Timestamp.Minutes.ToString("D3")}:{Timestamp.Seconds.ToString("D2")}:{Timestamp.Milliseconds.ToString("D3")}",  OutputFormat.Grey | OutputFormat.Italics);
        
        
        }

        static protected List<FormattedMessage> formatMessage(string message, LogLevel level, OutputFormat format = OutputFormat.Default)
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


    public partial class ContextLogger : Logger
    {  

        string _nodename;

        string _identifier;


        public ContextLogger(string Nodename, string Identifier)
        {
            _nodename = Nodename;
            _identifier = Identifier;

        }


        public new void Critical(string message)
        {   
            constructMessage(message, LogLevel.Critical);
        }
        public new void Warning(string message)
        {   
            constructMessage(message, LogLevel.Warnings);
        }
        public new void Debug(string message)
        {   
            constructMessage(message, LogLevel.Debug);
        }
        public new void Information(string message)
        {   
            constructMessage(message, LogLevel.Information);
        }
        public new void Important(string message)
        {   
            constructMessage(message, LogLevel.Important);
        }
        

        

        void constructMessage(string message, LogLevel level)
        {
            var formattedMessage = formatMessage(message,level);

            var injectedMessage = injectMetadata(formattedMessage);
            
            
            outputMessage(injectedMessage,level);

        }
        
    
        List<FormattedMessage> injectMetadata(List<FormattedMessage> message)
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



    
}