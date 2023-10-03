
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
    
    public interface ILogger
    {







        public void Critical(string message);
        public void Warning(string message);
        public void Debug(string message);
        public void Important(string message);
        public void Information(string message);
        public void Command(string message, string parameters = "", bool showTimestamp = true);


        public void Runtime(string label,string message, string parameters="");

       

    }




    
}