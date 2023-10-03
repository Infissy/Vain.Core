using System;

namespace Vain.Singleton
{
    [Serializable]
    public class RequiredSingletonMissingException : Exception
    {
        
        public RequiredSingletonMissingException(string singletonKey, Type type) : base($"Missing exception of type {type.Name} associated to '{singletonKey}'") { }

        protected RequiredSingletonMissingException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}