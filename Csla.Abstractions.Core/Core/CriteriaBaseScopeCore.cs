using System;
using Csla.Abstractions.Core.Contracts;
using Autofac;
using Csla.Abstractions.Attributes;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public class CriteriaBaseScopeCore<T> : CriteriaBaseCore<T>, IBusinessScope where T : CriteriaBaseScopeCore<T>
    {
        [NonSerialized]
        private ILifetimeScope scope;

        [Dependency]
        public ILifetimeScope Scope
        {
            get { return this.scope; }
            set { this.scope = value; }
        }
    }  
}
