using Godot;
using System.Collections.Generic;
using System.Linq;
using Vain.CLI;
using Vain.Log;


namespace Vain.Console
{

    ///<summary>
    /// GameConsole is the main  <see cref="P:Godot.Node"/> that contains the logic for the output.
    ///</summary>
    public partial class GameConsole : Node, IFormattedOutput
    {
        
        [Signal]
        public delegate void OnUpdateEventHandler();
        


        LineEdit _inputBox;
        bool _historyMode = false;

        internal ConsoleBuffer History {get;set;} = ConsoleBuffer.Instance;        


        public List<string> _buffer = new List<string>();

        public List<string> Buffer 
        {
            get { return _buffer; }
            
        } 


        public  override void _Ready(){

            //TODO : Change format, specially for build releases where debug stuff should not be showed
            Logger.GlobalLogger.RegisterOutput(this, (LogLevel) 63 );
            
            
            var button = GetNode("VBoxContainer/HBoxContainer/Button") as Button;
            _inputBox = GetNode("VBoxContainer/HBoxContainer/LineEdit") as LineEdit;



            button.Pressed += buttonPressed;
            _inputBox.TextSubmitted += buttonPressed;

        
            _inputBox.TextChanged += (string text) => 
                {
                    _historyMode = text == History.Current() ? true : false;
                    History.Reset();
                };
        }
        

        public override void _Input(InputEvent @input)
        {

            if(_inputBox.Text.Length == 0 || _historyMode)
            {
                

                //FIXME: Remove static action reference, move it all into one placee 
                if (@input.IsActionPressed("ui_up"))
                {
                    _inputBox.Text = History.Back();
                    _historyMode = true;
                }

                if(@input.IsActionPressed("ui_down"))
                {
                    _inputBox.Text = History.Forward();
                    _historyMode = true;
                }
            
            }
        }

        public void buttonPressed(){
            
            
            buttonPressed(_inputBox.Text);
        }


        public void buttonPressed(string text){

            
            if(text.Length == 0)
                return;


            var parsed = text.Split(' ');
                
            CommandRunner.Instance.Run(text);
            History.Add(text);
            _inputBox.Clear();
        
        }

        public void Write(string output)
        {
            _buffer.Add(output);
           
            EmitSignal(SignalName.OnUpdate);
        }

        public void Write(FormattedMessage[] formattedOutput)
        {
            
            var message ="";
            foreach (var messageSegment in formattedOutput)
            {
                message +=  $" {parseMessage(messageSegment.Message,messageSegment.Format)}";
            }

            

            _buffer.Add(message);
            EmitSignal(SignalName.OnUpdate);
        }


        string parseMessage(string message, OutputFormat format){
            
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

            parsedMessage = $"[color=#{color.ToHtml(false)}]{parsedMessage}[/color]";

            
         


            return parsedMessage;
        }
    
    
    }





}    
