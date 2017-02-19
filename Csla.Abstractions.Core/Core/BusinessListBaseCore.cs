using System;
using Csla.Abstractions.Core.Contracts;
using Csla.Core;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class BusinessListBaseCore<T,C> : BusinessListBase<T,C>, IBusinessListBaseCore<C> where T : BusinessListBaseCore<T,C> where C : IEditableBusinessObject
    {
    }
}
