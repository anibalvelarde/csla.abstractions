using System;
using Csla.Abstractions.Core.Contracts;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class ReadOnlyBaseCore<T> : ReadOnlyBase<T>, IReadOnlyBaseCore where T : ReadOnlyBaseCore<T>
    {
        protected ReadOnlyBaseCore()
            : base()
        {

        }
    }
}
