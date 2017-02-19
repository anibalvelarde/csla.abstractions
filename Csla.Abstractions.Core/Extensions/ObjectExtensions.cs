using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Csla.Abstractions.Extensions
{
    public static class ObjectExtensions
    {
        public static List<PropertyInfo> ReflectProperties(this object obj)
        {
            return obj.GetType().GetProperties().ToList<PropertyInfo>();
        }

        public static List<PropertyInfo> ReflectProperties(this object obj, BindingFlags reflectionBindingFlags)
        {
            return obj.GetType().GetProperties(reflectionBindingFlags).ToList<PropertyInfo>();
        }
    }
}
