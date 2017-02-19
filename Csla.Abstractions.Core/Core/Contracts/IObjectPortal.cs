namespace Csla.Abstractions.Core.Contracts
{
    public interface IObjectPortal<T> : IDataPortal<T>
    {
        T CreateChild();
        T CreateChild(params object[] args);
        T FetchChild();
        T FetchChild(params object[] args);
    }
}
