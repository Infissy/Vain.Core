
using System;

namespace Vain
{
    class ComponentNotFoundException<T> : Exception
    {
        public ComponentNotFoundException() : base($"Component of type {typeof(T).Name} not found.") {}
        
    }
}