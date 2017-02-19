using Csla.Abstractions.Attributes;
using Autofac;
using Autofac.Core;
using Csla.Abstractions.Core.Contracts;
using Csla.Abstractions.Extensions;
using Csla.Serialization.Mobile;
using System;
using System.Linq;
using System.Reflection;

namespace Csla.Abstractions
{
    /// <summary>
    /// utility class that handles setting all the dependency properties on a given business object or command
    /// </summary>
    public static class DependencyPropertySetter
    {
        /// <summary>
        /// used to access private and public nonstatic properties on business objects that have Dependency Attributes applied
        /// </summary>
        private const BindingFlags DPABindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static void InitializeDependencies(this IMobileObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Injection of dependencies cannot be performed in a 'null' instance.");
            }

            IBusinessScope scopedObject = obj as IBusinessScope;

            if (scopedObject != null)
            {
                if (scopedObject.Scope == null)
                {
                    throw new ArgumentNullException("obj", "An instance of IMobileObject that also implements IBusinessScope needs to have the Scope set before you can call InitializeDependencies on it!");
                }

                scopedObject.ReflectProperties(DPABindingFlags)
                    .Where(pi => Attribute.IsDefined(pi, typeof(DependencyAttribute)))
                    .ToList<PropertyInfo>()
                    .ForEach(pi =>
                    {
                        DependencyAttribute atb = pi.GetCustomAttribute<DependencyAttribute>();

                        if (scopedObject.Scope.IsRegistered(pi.PropertyType))
                        {
                            object depPropInstance = scopedObject.Scope.Resolve(pi.PropertyType);

                            if ((atb.DependencyScope == ResolutionScope.Client || atb.DependencyScope == ResolutionScope.ClientAndServer) && typeof(IDisposable).IsAssignableFrom(depPropInstance.GetType()))
                            {
                                IComponentRegistration reg = scopedObject.Scope
                                                                         .ComponentRegistry
                                                                         .RegistrationsFor(new TypedService(pi.PropertyType))
                                                                         .Where(e => e.Ownership == InstanceOwnership.ExternallyOwned).FirstOrDefault();
                                if (reg == null)
                                {
                                    throw new ConcreteTypeResolutionException(obj.GetType(), "Did not find a registration for given type.");
                                }
                            }

                            pi.SetValue(obj, depPropInstance);
                        }
                    });
            }
        }

        //disposes the scoped objects ILifetimeScope and sets all dependency properties to null
        public static void FinalizeDependencies(this IMobileObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Object to be processed for dependency injection cannot be 'null'.");
            }

            IBusinessScope scopedObject = obj as IBusinessScope;

            if (scopedObject != null)
            {
                if (scopedObject.Scope != null)
                {
                    //if the scope is disposed the dependency that was resolved from it will get disposed as well - 
                    //we need to ensure that the dependencies we bring back from the client do NOT implememnt IDisposable
                    //OR - if they do - we need to ensure that the container was registered to not track the client dependency
                    //(this happens in the 
                    scopedObject.Scope.Dispose();
                    scopedObject.Scope = null;

                    scopedObject.ReflectProperties(DPABindingFlags)
                        .Where(pi => Attribute.IsDefined(pi, typeof(DependencyAttribute)))
                        .ToList<PropertyInfo>()
                        .ForEach(pi =>
                        {
                            DependencyAttribute atb = pi.GetCustomAttribute<DependencyAttribute>();
                            //we only clear out the scoped object if it is only used on the server side
                            //we checked for IDisposable and made sure that the client side dependency was not being tracked by 
                            //the server side resolution in Autofac when InitializeObject was called so we will be okay to not clear it out
                            //and do not need to worry about it getting disposed here as well
                            if (atb.DependencyScope == ResolutionScope.Server)
                            {
                                pi.SetValue(scopedObject, null);
                            }
                        });
                }
            }
        }
    }
}
