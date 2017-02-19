using System;

namespace Csla.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DependencyAttribute : Attribute
    {
        private ResolutionScope _depResScope = ResolutionScope.Server;

        public DependencyAttribute() { }

        public DependencyAttribute(ResolutionScope aDependencyResolutionScope)
        {
            _depResScope = aDependencyResolutionScope;
        }

        /// <summary>
        /// used by the ObjectPortal and the DataPortalActivator to determine when to resolve the DependencyProperty of your Csla Business Object
        /// </summary>
        public ResolutionScope DependencyScope
        {
            get { return _depResScope; }
        }
    }
}
