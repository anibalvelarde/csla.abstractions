using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Csla.Abstractions.Extensions
{
    /// <summary>
    /// Resolves the concrete type for an interface if one is passed in - otherwise tries to resolve the type
    /// from what is currently loaded in the AppDomain - if the App domain does not contain an assembly
    /// that has a concrete implementation of the passed in interface - we throw an exception
    /// </summary>
    /// <remarks>
    /// Will throw an exception if the type cannot be found or finds more than one implementation in either the current assembly or 
    /// in one or more other loaded assemblies of the current application domain
    /// </remarks>
    /// <exception cref="Csla.Abstractions.ConcreteTypeResolutionException"></exception>
    public static class TypeExtensions
    {
        /// <summary>
        /// scans for the first available implementation of a given interface type from whatever assembly this method is called from
        /// if the current assembly does not have a concrete implmentation - this method will scan all the assemblies in the current appdomain
        /// looking for one that has it - if more than one implementation of the passed in interface type is found, this method will throw an exception
        /// notifying the caller that they need to resolve the type using an IoC container instead of relying on this method
        /// </summary>
        /// <returns>Concrete implementation type for a given passed in interface, if you pass in null or a concrete type, 
        /// it will return the type or null reference that you passed in</returns>
        /// <exception cref="ConcreteTypeResolutionException">Thrown when you have more than one implementation of the passed in interface or abstract type in memory</exception>
        public static Type ScanForFirstConcreteType(this Type @this)
        {
            //if we are given a concrete type or null value to start with - return either null or that same type back
            //if we are given an interface or abstract class - and the current type has a namespace 
            //then scan for the first available implementation (looking in the same assembly as the passed in type first) then broadening that search
            //to the other assemblies in the current app domain
            if (@this == null || !@this.IsInterface || !@this.IsAbstract || string.IsNullOrWhiteSpace(@this.Namespace))
            {
                return @this;
            }
            else
            {
                //check for an implementation in the current assembly first, 
                //if we find more than one implementation of the interface or abstract base type - throw an exception (this scanner will not perform explicit binding and is not meant to)
                //if there is no implementation available in the current assembly
                //move on to checking all the currently loaded assemblies in the current app domain
                //if more than one implementation is found across all the assemblies in the app domain - throw an exception (again this scanner does not do explicit binding - if you use this then it assumes you only have one implementation - this is by design)
                //if only one is found - then return the type that implements the passed in interface / abstract type that is 
                //not an interface OR an abstract type (must be a concreate type)

                Assembly curAsm = Assembly.GetAssembly(@this);
                IEnumerable<Type> availableConcreteTypes = curAsm.GetTypes().Where(t => @this.IsAssignableFrom(t) & !t.IsInterface & !t.IsAbstract & !t.IsSubclassOf(@this));
                Type retType = null;

                if (availableConcreteTypes.Count().Equals(2))
                {
                    var item1 = availableConcreteTypes.First();
                    var item2 = availableConcreteTypes.Last();

                    if (item1.IsSubclassOf(item2)) 
                    { 
                        retType = item2; 
                    }
                    else if (item2.IsSubclassOf(item1))
                    {
                        retType = item1;
                    }
                    else
                    {
                        throw new ConcreteTypeResolutionException(@this, string.Format("Found more than one implementation of type [{0}] in assembly [{1}].", @this.FullName, curAsm.FullName));
                    }
                }
                else if (availableConcreteTypes.Count() > 2) //we found more than one implementation of the interface in the current assembly - throw an exception to force developers to provide a container that specifies a binding
                {
                    throw new ConcreteTypeResolutionException(@this, string.Format("Found more than one implementation of type [{0}] in assembly [{1}].", @this.FullName, curAsm.FullName));
                }
                else if (availableConcreteTypes.Count() == 1)
                {
                    retType = availableConcreteTypes.First();
                }
                else // (.Count() returned zero - scan the rest of the app domain for the type)
                {
                    List<Assembly> implAsmList = AppDomain.CurrentDomain.GetAssemblies()
                                                                        .Where(a => a.GetTypes().Where(t => @this.IsAssignableFrom(t) & !t.IsInterface & !t.IsAbstract).Count() > 0).ToList();
                    if (implAsmList == null || implAsmList.Count == 0)
                    {
                        throw new ConcreteTypeResolutionException(@this, string.Format("Unable to locate a loaded assembly that has an implementation for the requested service type of {0} in the current application domain.", @this.FullName));
                    }
                    else if (implAsmList.Count > 1)
                    {
                        throw new ConcreteTypeResolutionException(@this, string.Format("Unable to resolve a single assembly that has an implementation for the requested service type of {0} in the current application domain, use the IoC container to resolve the correct type for this service instead.", @this.FullName));
                    }
                    else
                    {
                        Assembly tpAsm = implAsmList.First();
                        List<Type> locatedTypes = tpAsm.GetTypes()
                                                       .Where(t => @this.IsAssignableFrom(t) & !t.IsInterface & !t.IsAbstract).ToList();

                        if (locatedTypes == null || locatedTypes.Count == 0)
                        {
                            throw new ConcreteTypeResolutionException(@this, string.Format("Unable to locate implementation in the assembly {0} that has an implementation for the requested service type of {1} in the current application domain.", tpAsm.FullName, @this.FullName));
                        }
                        else if (locatedTypes.Count > 1)
                        {
                            throw new ConcreteTypeResolutionException(@this, string.Format("Unable to resolve a single type that implements the requested service type of {0} in the assembly {1}, use the IoC container to resolve the correct type for this service instead.", @this.FullName, tpAsm.FullName));
                        }
                        else
                        {
                            retType = locatedTypes.First();
                        }
                    }
                }

                return retType;
            }
        }
    }
}
