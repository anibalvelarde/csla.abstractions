using System;
using Csla.Abstractions.Attributes;
using Csla.Abstractions.Core.Contracts;
using Autofac;
using Csla.Core;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class BusinessListBaseScopeCore<T, C> : BusinessListBaseCore<T, C>, IBusinessScope where T : BusinessListBaseScopeCore<T,C> 
                                                                                                         where C : IEditableBusinessObject
    {
        protected BusinessListBaseScopeCore()
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
