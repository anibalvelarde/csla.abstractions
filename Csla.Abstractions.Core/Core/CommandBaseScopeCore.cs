using System;
using Csla.Abstractions.Core.Contracts;
using Csla.Abstractions.Attributes;
using Autofac;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class CommandBaseScopeCore<T> : CommandBaseCore<T>, IBusinessScope where T : CommandBaseScopeCore<T>
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
