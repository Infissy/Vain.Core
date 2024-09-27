using System;

namespace Vain;



[AttributeUsage(AttributeTargets.Class)]
class NodeIndexingAttribute : Attribute
{
    public string Key;
    public NodeIndexingAttribute(string key)
    {
        Key = key;
    }
}
