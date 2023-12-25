namespace Vain.Log;

public interface IFormattedOutput : IOutput
{
    void Write(FormattedMessage[] formattedOutput);

}

