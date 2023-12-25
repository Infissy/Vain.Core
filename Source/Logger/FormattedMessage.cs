namespace Vain.Log;

public struct FormattedMessage
{
    public string Message;
    public OutputFormat Format;


    public FormattedMessage(string message, OutputFormat format) {

        Message = message;
        Format = format;

    }

}
