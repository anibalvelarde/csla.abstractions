using System;
using Csla.Abstractions.Attributes;
using Csla.Abstractions.Core.Contracts;
using Autofac;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class BusinessBaseScopeCore<T> : BusinessBaseCore<T>, IBusinessScope where T : BusinessBaseScopeCore<T>
    {
        protected BusinessBaseScopeCore()
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
