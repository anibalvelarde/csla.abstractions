using System;
using System.Linq.Expressions;
using Csla.Core;
using Csla.Reflection;

namespace Csla.Abstractions.Utils
{
    [Serializable]
    public sealed class PropertyInfoRegistration : BusinessBase
    {
        public PropertyInfoRegistration()
            : base()
        { }

        public static PropertyInfo<T> Register<TTarget, T>(Expression<Func<TTarget, object>> propertyLambdaExpression)
        {
            var property = new PropertyInfo<T>(Reflect<TTarget>.GetProperty(propertyLambdaExpression).Name);
            return BusinessBase.RegisterProperty<T>(typeof(TTarget), property);
        }

        public static PropertyInfo<T> Register<TTarget, T>(Expression<Func<TTarget,
            object>> propertyLambdaExpression, RelationshipTypes relationship)
        {
            var property = new PropertyInfo<T>(
                Reflect<TTarget>.GetProperty(propertyLambdaExpression).Name, relationship);
            return BusinessBase.RegisterProperty<T>(typeof(TTarget), property);
        }

        public static PropertyInfo<T> Register<T>(Type targetType, string name)
        {
            var property = new PropertyInfo<T>(name);
            return BusinessBase.RegisterProperty<T>(targetType, property);
        }
    }
}
