
namespace Vain.Log;

public interface ILogger
{
    void Critical(string message);
    void Warning(string message);
    void Debug(string message);
    void Important(string message);
    void Information(string message);
    void Command(string message, string parameters = "", bool showTimestamp = true);
    void Runtime(string label,string message, string parameters="");
}
