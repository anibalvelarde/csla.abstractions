using System;
using Csla.Abstractions.Attributes;
using Csla.Abstractions.Core.Contracts;
using Autofac;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class ReadOnlyListBaseScopeCore<T, C> : ReadOnlyListBaseCore<T, C>, IBusinessScope where T : ReadOnlyListBaseScopeCore<T,C> 
    {
        protected ReadOnlyListBaseScopeCore()
            : base()
        {

        }

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
