using System;

namespace Csla.Abstractions
{
    [Serializable]
    public class ConcreteTypeResolutionException : Exception
    {
        public ConcreteTypeResolutionException(Type requestedServiceType, string message)
            : base(string.Format("{0} [Type: {1}]",message, requestedServiceType.Name))
        {
        }

        public ConcreteTypeResolutionException(Type requestedServiceType, string message, Exception innerEx)
            : base(string.Format("{0} [Type: {1}]", message, requestedServiceType.Name), innerEx)
        {
        }
    }
}
