using System;
using System.Threading.Tasks;
using Csla.Abstractions.Core.Contracts;
using Csla.Core;
using Csla.Serialization.Mobile;

namespace Csla.Abstractions
{

    public class ObjectPortal<T>
    : IObjectPortal<T>
    where T : class, IMobileObject
    {
#pragma warning disable 67

        public void BeginCreate(object criteria, object userState)
        {
            throw new NotImplementedException();
        }

        public void BeginCreate(object criteria)
        {
            throw new NotImplementedException();
        }

        public void BeginCreate()
        {
            throw new NotImplementedException();
        }

        public void BeginDelete(object criteria, object userState)
        {
            throw new NotImplementedException();
        }

        public void BeginDelete(object criteria)
        {
            throw new NotImplementedException();
        }

        public void BeginExecute(T command, object userState)
        {
            throw new NotImplementedException();
        }

        public void BeginExecute(T command)
        {
            throw new NotImplementedException();
        }

        public void BeginFetch(object criteria, object userState)
        {
            throw new NotImplementedException();
        }

        public void BeginFetch(object criteria)
        {
            throw new NotImplementedException();
        }

        public void BeginFetch()
        {
            throw new NotImplementedException();
        }

        public void BeginUpdate(T obj, object userState)
        {
            throw new NotImplementedException();
        }

        public void BeginUpdate(T obj)
        {
            throw new NotImplementedException();
        }

        public T Create()
        {
            return DataPortal.Create<T>();
        }

        public T Create(object criteria)
        {            
            return DataPortal.Create<T>(criteria);
        }

        public Task<T> CreateAsync(object criteria)
        {
            return DataPortal.CreateAsync<T>(criteria);
        }

        public Task<T> CreateAsync()
        {
            return DataPortal.CreateAsync<T>();
        }

        public event EventHandler<DataPortalResult<T>> CreateCompleted;

        public T CreateChild()
        {
            return DataPortal.CreateChild<T>();
        }

        public  T CreateChild(params object[] args)
        {
            return DataPortal.CreateChild<T>(args);
        }

        public void Delete(object criteria)
        {
            DataPortal.Delete<T>(criteria);
        }

        public Task DeleteAsync(object criteria)
        {
            return DataPortal.DeleteAsync<T>(criteria);
        }

        public event EventHandler<DataPortalResult<T>> DeleteCompleted;

        public T Execute(T obj)
        {
            return DataPortal.Execute<T>(obj);
        }

        public Task<T> ExecuteAsync(T command)
        {
            return DataPortal.ExecuteAsync<T>(command);
        }

        public event EventHandler<DataPortalResult<T>> ExecuteCompleted;

        public T Fetch()
        {
            return DataPortal.Fetch<T>();
        }

        public T Fetch(object criteria)
        {
            return DataPortal.Fetch<T>(criteria);
        }

        public Task<T> FetchAsync()
        {
            return DataPortal.FetchAsync<T>();
        }

        public Task<T> FetchAsync(object criteria)
        {
            return DataPortal.FetchAsync<T>(criteria);
        }

        public T FetchChild()
        {
            return DataPortal.FetchChild<T>();
        }

        public T FetchChild(params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            return DataPortal.FetchChild<T>(args);
        }

        public event EventHandler<DataPortalResult<T>> FetchCompleted;

        public ContextDictionary GlobalContext
        {
            get { return ApplicationContext.GlobalContext; }
        }

        public T Update(T obj)
        {
            return DataPortal.Update<T>(obj);
        }

        public Task<T> UpdateAsync(T obj)
        {
            return DataPortal.UpdateAsync<T>(obj);
        }

        public event EventHandler<DataPortalResult<T>> UpdateCompleted;

#pragma warning restore 67
    }

}
