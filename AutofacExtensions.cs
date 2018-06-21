using System;
using System.Linq;
using System.Reflection;
using Autofac;

namespace Automa.Entities.Unity.Autofac
{
    public static class AutofacExtensions
    {
        public static void Inject(this IComponentContext context, object instance)
        {
            InjectProperties(context, instance);
            InjectFields(context, instance);
        }

        public static void InjectProperties(this IComponentContext context, object instance)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            var type = instance.GetType();
            foreach (var propertyInfo in type.GetRuntimeProperties().Where(pi => pi.CanWrite))
            {
                var propertyType = propertyInfo.PropertyType;
                var injectAttribute = propertyInfo.GetCustomAttribute<AutoInject>();
                if (injectAttribute != null
                    && (!propertyType.GetTypeInfo().IsValueType || propertyType.GetTypeInfo().IsEnum)
                    && (!propertyType.IsArray || !propertyType.GetElementType().GetTypeInfo().IsValueType)
                    && propertyInfo.GetIndexParameters().Length == 0)
                {
                    propertyInfo.SetValue(instance, injectAttribute.Key != null
                        ? context.ResolveKeyed(injectAttribute.Key, propertyType)
                        : context.Resolve(propertyType), null);
                }
            }
        }

        public static void InjectFields(this IComponentContext context, object instance)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            var type = instance.GetType();
            foreach (var fieldInfo in type.GetRuntimeFields())
            {
                var propertyType = fieldInfo.FieldType;
                var injectAttribute = fieldInfo.GetCustomAttribute<AutoInject>();
                if (injectAttribute != null
                    && (!propertyType.GetTypeInfo().IsValueType || propertyType.GetTypeInfo().IsEnum)
                    && (!propertyType.IsArray || !propertyType.GetElementType().GetTypeInfo().IsValueType))
                {
                    fieldInfo.SetValue(instance, injectAttribute.Key != null
                        ? context.ResolveKeyed(injectAttribute.Key, propertyType)
                        : context.Resolve(propertyType));
                }
            }
        }
    }
}