using System;

namespace Vain.Core;

//Check Component class for explanation

[AttributeUsage(AttributeTargets.Field)]
class RequiredChildAttribute : Attribute
{
    public string Message{get;set;}
    public RequiredChildAttribute(String optionalMessage = "")  =>  Message = optionalMessage;
}

