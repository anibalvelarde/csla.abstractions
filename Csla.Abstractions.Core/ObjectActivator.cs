using System;
using System.Linq;
using System.Reflection;
using Csla.Abstractions.Attributes;
using Csla.Abstractions.Core.Contracts;
using Autofac;
using Csla.Server;
using Autofac.Core;
using Csla.Reflection;
using Csla.Serialization.Mobile;

namespace Csla.Abstractions
{
    public sealed class ObjectActivator :IDataPortalActivator
    {
        private IContainer _iocContainer;
        public ObjectActivator(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this._iocContainer = container;
        }

        public object CreateInstance(Type requestedType)
        {
            if (requestedType == null)
            {
                throw new ArgumentNullException("requestedType");
            }
            else
            {
                object retInst = null;
                ILifetimeScope scope = null;

                if ((requestedType.IsInterface || requestedType.IsAbstract) && (!_iocContainer.ComponentRegistry.IsRegistered(new TypedService(requestedType))))
                {
                    throw new ConcreteTypeResolutionException( requestedType, string.Format("Missing IoC Type Registration for type '{0}'.", requestedType.FullName));
                }
                else if (!_iocContainer.ComponentRegistry.IsRegistered(new TypedService(requestedType)))
                {
                    retInst = MethodCaller.CreateInstance(requestedType);
                }
                else
                {
                    scope = _iocContainer.BeginLifetimeScope();
                    retInst = scope.Resolve(requestedType);
                }

                var scopeRetInst = retInst as IBusinessScope;

                if (scopeRetInst != null)
                {
                    scopeRetInst.Scope = scope ?? _iocContainer.BeginLifetimeScope();
                }
                return retInst;
            }
        }

        public void InitializeInstance(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.Format("Initialization request cannot be processed for a 'null' instance."));
            }

            IBusinessScope scopedObject = obj as IBusinessScope;

            if (scopedObject != null)
            {
                if (scopedObject.Scope == null)
                {
                    scopedObject.Scope = _iocContainer.BeginLifetimeScope();
                }

                ((IMobileObject)scopedObject).InitializeDependencies();
            }
        }

        public void FinalizeInstance(object obj)        
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var scopedObject = obj as IBusinessScope;

            if (scopedObject != null)
            {
                scopedObject.Scope.Dispose();

                foreach (var property in
                    (from _ in scopedObject.GetType().GetProperties(
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                     where _.GetCustomAttribute<DependencyAttribute>() != null
                     select _))
                {
                    property.SetValue(scopedObject, null);
                }
            }
        }
    }
}
