using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Godot;


namespace Vain.Console
{
    internal partial class ConsoleBuffer : Resource
    {

        const int HISTORY_SIZE = 300;
        private static ConsoleBuffer _instance;
        
        public static ConsoleBuffer Instance 
        {
            get
            {


                if(_instance == null)
                {
                    var path = ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.SavesFolder) + "/ConsoleHistory.tres";
                    if(!ResourceLoader.Exists(path))
                    {
                        ResourceSaver.Save(new ConsoleBuffer(),path);
                    }
                    _instance = ResourceLoader.Load<ConsoleBuffer>(path);
                    _instance.Reset();
                }

                return _instance;

            }
        }


        int _pointer;

        [Export]
        protected Godot.Collections.Array<string> History {get; private set;} = new(); 
        

        public void Add(string command)
        {
            if(History.Count == HISTORY_SIZE)
                History.RemoveAt(History.Count - 1);
            
            if(History.Last() != command)
                History.Add(command);
            Reset();
        
            var path = ProjectConfig.LoadConfiguration(ProjectConfig.SingleSourceConfiguration.SavesFolder) + "/ConsoleHistory.tres";
            ResourceSaver.Save(this,path);
        
        }


        public string Back()
        {

            if(History.Count == 0)
                return "";
            
            
            
            _pointer  = Mathf.Max(0 , _pointer - 1);

            return History[_pointer];
        }

        public string Forward()
        {

            _pointer  = Mathf.Min(History.Count, _pointer + 1);


            if(History.Count == 0 || History.Count == _pointer)
                return "";


            return History[_pointer];
        }


        public string Current()
        {
            if(History.Count == 0 || History.Count == _pointer)
                return "";
           

            return History[_pointer];
        }

        public void Reset()
        {
            _pointer =  History.Count;
        }

    }

}