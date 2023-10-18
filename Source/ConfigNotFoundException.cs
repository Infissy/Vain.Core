using System;

namespace Vain
{
    public class ConfigNotFoundException : Exception
    {

        public ConfigNotFoundException(string section, string key) : base($"Config of type {section}/{key} not found in loaded configuration files.") {}
    }
}