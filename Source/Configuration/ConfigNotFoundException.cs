using System;

namespace Vain.Configuration;

public class ConfigNotFoundException : Exception
{

    public ConfigNotFoundException(string section, string key) : base($"Config of type {section}/{key} not found in loaded configuration files.") {}

    ConfigNotFoundException(){}

    ConfigNotFoundException(string message) : base(message){}

    ConfigNotFoundException(string message, Exception innerException) : base(message, innerException){}
}
