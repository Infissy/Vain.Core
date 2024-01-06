using System;
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



    public void LoadHistory()
    {
        var path = GetSavePath();

        var rawHistory = FileAccess.GetFileAsString(path);

        History = (Json.ParseString(rawHistory).Obj as Godot.Collections.Array<string>) ?? new Godot.Collections.Array<string>();

    }

    public void SaveHistory()
    {
        var path = GetSavePath();

        var rawHistory = Json.Stringify(History);

        var file = FileAccess.Open(path,FileAccess.ModeFlags.Write);

        file.StoreString(rawHistory);

    }


    public static string GetSavePath()
    {
        var saveFolderPath = ProjectConfiguration.LoadConfiguration(ProjectConfiguration.SingleSourceConfiguration.SavesFolder);

        DirAccess.MakeDirAbsolute(saveFolderPath);

        return saveFolderPath  + "/console_history.json";

    }
}

