using System;
using Csla.Abstractions.Core.Contracts;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class CriteriaBaseCore<T> : CriteriaBase<T>, ICriteriaBaseCore where T : CriteriaBaseCore<T>        
    {
        protected CriteriaBaseCore()
            : base()
        {

        }
    }
}
