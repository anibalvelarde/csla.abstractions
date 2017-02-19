using Autofac;

namespace Csla.Abstractions.Core.Contracts
{
    internal interface IBusinessScope
    {
        ILifetimeScope Scope { get; set; }
    }
}
