using System;
using Csla.Abstractions.Core.Contracts;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class ReadOnlyListBaseCore<T,C> : ReadOnlyListBase<T,C>, IReadOnlyListBaseCore<C> where T : ReadOnlyListBaseCore<T,C>
    {

    }
}
