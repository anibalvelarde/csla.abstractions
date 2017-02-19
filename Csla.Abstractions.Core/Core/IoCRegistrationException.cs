using System;
using System.Runtime.Serialization;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public class IoCRegistrationException : Exception
    {
        public IoCRegistrationException()
        {
        }

        public IoCRegistrationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IoCRegistrationException(string message)
            : base(message)
        {
        }

        public IoCRegistrationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    public IoCRegistrationException(string dependency, string dependentClassName, string dependentPropertyName)
            : base(string.Format("Missing IoC registration for type [{0}] to be injected in [{1}.{2}].", 
                                    dependency, dependentClassName, dependentPropertyName))
        {
            
        }

    public IoCRegistrationException(string dependency, string dependentClassName, string dependentPropertyName, Exception inner)
        : base(string.Format("Missing IoC registration for type [{0}] to be injected in [{1}.{2}].",
                                dependency, dependentClassName, dependentPropertyName), inner)
        {

        }
    }
}
