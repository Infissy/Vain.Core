using System;

namespace Vain;


[AttributeUsage(AttributeTargets.Class)]
class SceneBindingAttribute : Attribute
{

    public string Path;

    public SceneBindingAttribute(string path)
    {
        Path = path;
    }
}