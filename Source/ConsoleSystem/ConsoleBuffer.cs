using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Godot;
using Vain.Configuration;


namespace Vain.Console;

internal partial class ConsoleBuffer : Resource
{
    const int HISTORY_SIZE = 300;
    int _pointer;



    private static ConsoleBuffer _instance;

    public static ConsoleBuffer Instance
    {
        get
        {


            if(_instance == null)
            {
                var saveFolder = ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.SavesFolder);

                DirAccess.MakeDirAbsolute(saveFolder);


                var path = saveFolder  + "/ConsoleHistory.tres";
                if(!ResourceLoader.Exists(path))
                {
                    ResourceSaver.Save(new ConsoleBuffer(),path);
                }
                _instance = ResourceLoader.Load<ConsoleBuffer>(path);
                _instance.ResetPointer();
            }

            return _instance;

        }
    }



    [Export]
    protected Godot.Collections.Array<string> History {get; private set;} = new();

    public void Add(string command)
    {
        if(History.Count == HISTORY_SIZE)
            History.RemoveAt(History.Count - 1);

        if(History.Count == 0 || History.Last()  != command)
            History.Add(command);

        ResetPointer();

        var path = ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.SavesFolder) + "/ConsoleHistory.tres";

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

    public void ResetPointer()
    {
        _pointer =  History.Count;
    }

}

