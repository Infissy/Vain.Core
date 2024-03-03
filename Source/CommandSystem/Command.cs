using System;

using System.Globalization;
using System.Linq;


namespace Vain.CLI;



public class Command
{
    public string[] Path { get; }
    public Delegate Action { get; }
    public Type[] ListenerType { get;}

    /// <summary>
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action">It's possible to define async functions but they need to return a value, void functions can induce unexpected behaviours.</param>
    public Command(string path, Delegate action)
    {
        Path = path.Split(' ',StringSplitOptions.RemoveEmptyEntries);

        Action = action;
    }
  

    static class InputType
    {
        public const string FLOAT = "?:f";
        public const string INT = "?:n";
        public const string STRING = "?:s";
        public const string CONST_STRING = "c:s";
    }



    public bool TryParseParameters(string[] parameters, out object[] parsedParameters)
    {
        parsedParameters = null;

        if(parameters.Length != Path.Length)
            return false;

        var localParameters = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            switch (Path[i])
            {
                case InputType.FLOAT:

                    float f;
                    if(!float.TryParse(parameters[i],NumberStyles.Float | NumberStyles.AllowThousands,CultureInfo.GetCultureInfo("en-US"), out f))
                        return false;

                    localParameters[i] = f;
                    break;

                case InputType.INT:

                    int n;
                    if(!int.TryParse(parameters[i],NumberStyles.Integer,CultureInfo.GetCultureInfo("en-US"), out n))
                        return false;

                    localParameters[i] = n;
                    break;

                case InputType.STRING:

                    localParameters[i] = parameters[i];

                    break;

                default:
                    if(parameters[i] != Path[i])
                        return false;
                    localParameters[i] = null;
                    break;
            }
        }

        parsedParameters = localParameters.Where(p => p != null).ToArray();
        return true;
    }
}
