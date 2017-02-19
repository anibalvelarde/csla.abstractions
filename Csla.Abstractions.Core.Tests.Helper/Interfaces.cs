using System;
using Csla.Abstractions.Core.Contracts;

namespace Csla.Abstractions.Tests.Helper
{
    public interface ImplementedOnceByTypeInDifferentAssembly
    {
        int aNumber { get; }
    }

    public interface ImplementedMultipleByTypeInDifferentAssembly
        : IBusinessBaseCore
    {
        string aString { get; }
    }

    public interface ImplementedByNobodyInDifferentAssembly
    {
        Guid anID { get; }
    }
}
