using Godot;
using System.Collections.Generic;
using System.Linq;
using Vain.Command;
using Vain.Log;


namespace Vain.Console
{

    ///<summary>
    /// GameConsole is the main  <see cref="P:Godot.Node"/> that contains the logic for the output.
    ///</summary>
    public class GameConsole : Node, IFormattedOutput
    {
        

        public delegate void UpdateHandler();
        public event UpdateHandler OnUpdate;


        LineEdit _inputBox;



        public List<string> _buffer = new List<string>();

        public List<string> Buffer 
        {
            get { return _buffer; }
            
        } 


        public  override void _Ready(){

            //TODO : Change format, specially for build releases where debug stuff should not be showed
            Logger.RegisterOutput(this, (LogLevel) 63 );
            
            
            var button = FindNode("Button") as Button;
            _inputBox = FindNode("LineEdit") as LineEdit;





            button.Connect("pressed",this,"buttonPressed");


            _inputBox.Connect("text_entered",this,"buttonPressed");
            
            
        }
        

        public void buttonPressed(){
            
            
            buttonPressed(_inputBox.Text);
        }


        public void buttonPressed(string text){

            
            if(text != "?")
            {

        
                var parsed = text.Split(' ');
                
                Runner.Instance.Run(parsed[0], parsed.Skip(1).ToArray());


                _inputBox.Clear();
            }
            else
            {
                var parsed = text.Split(' ');
                
                Runner.Instance.Run(parsed[0], parsed.Skip(1).ToArray());


                _inputBox.Clear();
            }

        }

        public void Write(string output)
        {
            _buffer.Add(output);
            if (OnUpdate == null) return;
                OnUpdate.Invoke();
        }

        public void Write(FormattedMessage[] formattedOutput)
        {
            
            var message ="";
            foreach (var messageSegment in formattedOutput)
            {
                message +=  $" {parseMessage(messageSegment.Message,messageSegment.Format)}";
            }

            

            _buffer.Add(message);
            if (OnUpdate == null) return;
                OnUpdate.Invoke();
        }


        string parseMessage(string message, OutputFormat format){
            
            string parsedMessage = message;

            var color = Colors.Black;


            if((format & OutputFormat.Bold) == OutputFormat.Bold)
                parsedMessage = $"[b]{parsedMessage}[/b]";
            if((format & OutputFormat.Italics) == OutputFormat.Italics)
                parsedMessage = $"[i]{parsedMessage}[/i]";

            

            if((format & OutputFormat.Red) == OutputFormat.Red)
                color.r = 255;
            if((format & OutputFormat.Blue) == OutputFormat.Blue)
                color.g = 255;
            if((format & OutputFormat.Green) == OutputFormat.Green)
                color.b = 255;
            if((format & OutputFormat.Grey) == OutputFormat.Grey)
                color = Colors.DarkSlateGray;



            if((format & FormatMasks.ColorMask ) == 0)
                color = Colors.White;

            parsedMessage = $"[color=#{color.ToHtml(false)}]{parsedMessage}[/color]";

            
         


            return parsedMessage;
        }
    
    
    }





}    
