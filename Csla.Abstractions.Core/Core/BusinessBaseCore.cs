using System;
using Csla.Abstractions.Core.Contracts;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class BusinessBaseCore<T> : BusinessBase<T>, IBusinessBaseCore where T : BusinessBaseCore<T>
    {
        protected BusinessBaseCore()
            : base()
        {

        }
    }
}
