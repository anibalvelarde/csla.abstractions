using System;
using Csla.Abstractions.Attributes;
using Csla.Abstractions.Core.Contracts;
using Autofac;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class ReadOnlyBaseScopeCore<T> : ReadOnlyBaseCore<T>, IBusinessScope where T : ReadOnlyBaseScopeCore<T>
    {
        protected ReadOnlyBaseScopeCore()
            : base() 
        {}

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
