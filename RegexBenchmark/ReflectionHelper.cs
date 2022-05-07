using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RegexBenchmark
{
    public class ReflectionHelper
    {
        private static readonly Type FunctionType = typeof(Func<,>);
        private static readonly Type ReflectionHelperType = typeof(ReflectionHelper);
        private static readonly ConcurrentDictionary<Type, ReflectionHelper[]> Cache
        = new ConcurrentDictionary<Type, ReflectionHelper[]>();
        private static readonly MethodInfo CallInnerDelegateMethod =
            ReflectionHelperType.GetMethod(nameof(CallInnerDelegate), BindingFlags.NonPublic | BindingFlags.Static);
        public PropertyInfo Property { get; set; }
        public Func<object, object> Getter { get; set; }

        // Called via reflection.
        private static Func<object, object> CallInnerDelegate<TClass, TResult>(
            Func<TClass, TResult> deleg)
            => instance => deleg((TClass)instance);

        public static ReflectionHelper[] GetProperties(Type type)
            => Cache
                .GetOrAdd(type, _ => type
                    .GetProperties()
                    .Select(property =>
                    {
                        var getMethod = property.GetMethod;
                        var declaringClass = property.DeclaringType;
                        var typeOfResult = property.PropertyType;
                        // Func<Type, TResult>
                        var getMethodDelegateType = FunctionType.MakeGenericType(declaringClass, typeOfResult);
                        // c => c.Data
                        var getMethodDelegate = getMethod.CreateDelegate(getMethodDelegateType);
                        // CallInnerDelegate<Type, TResult>
                        var callInnerGenericMethodWithTypes = CallInnerDelegateMethod
                                .MakeGenericMethod(declaringClass, typeOfResult);
                        // Func<object, object>
                        var result = (Func<object, object>)callInnerGenericMethodWithTypes.Invoke(null, new[] { getMethodDelegate });
                        //var setter = property.GetSetMethod();
                        return new ReflectionHelper
                        {
                            Property = property,
                            Getter = result,
                            //Setter = setter
                        };
                    })
                    .ToArray());
    }

    //===================================================================================

    //public static class ReflectionCache
    //{
    //    private static ConcurrentDictionary<Type, CachedReflectionType> cache = new ConcurrentDictionary<Type, CachedReflectionType>();

    //    public static FieldInfo[] GetFields(Type type)
    //    {
    //        return GetReflectedType(type).GetFields(); //GetFields, GetMethods, GetProperties
    //    }

    //    public static MethodInfo[] GetMethods(Type type)
    //    {
    //        return GetReflectedType(type).GetMethods();
    //    }

    //    public static FieldInfo GetField(Type type, string field)
    //    {
    //        return GetReflectedType(type).GetField(field);
    //    }

    //    public static MethodInfo GetMethod(Type type, string method)
    //    {
    //        return GetReflectedType(type).GetMethod(method);
    //    }

    //    public static PropertyInfo GetProperty(Type type, string property)
    //    {
    //        return GetReflectedType(type).GetProperty(property);
    //    }

    //    public static PropertyInfo[] GetProperties(Type type)
    //    {
    //        return GetReflectedType(type).GetProperties();
    //    }

    //    private static CachedReflectionType GetReflectedType(Type type)
    //    {
    //        return cache.GetOrAdd(type, new CachedReflectionType(type));
    //    }

    //}

    //public class CachedReflectionType
    //{
    //    private Type type;
    //    private FieldInfo[] allFields;
    //    private MethodInfo[] allMethods;
    //    private PropertyInfo[] allProperties;

    //    public CachedReflectionType(Type type)
    //    {
    //        this.type = type;
    //    }

    //    public FieldInfo[] GetFields()
    //    {
    //        if (allFields == null)
    //            allFields = type.GetFields();

    //        return allFields;
    //    }

    //    public MethodInfo[] GetMethods()
    //    {
    //        if (allMethods == null)
    //            allMethods = type.GetMethods();
    //        return allMethods;
    //    }

    //    public PropertyInfo[] GetProperties()
    //    {
    //        if (allProperties == null)
    //            allProperties = type.GetProperties();
    //        return allProperties;
    //    }

    //    public FieldInfo GetField(string field)
    //    {
    //        if (!fields.ContainsKey(field))
    //            fields.Add(field, type.GetField(field));

    //        return fields[field];
    //    }

    //    public MethodInfo GetMethod(string method)
    //    {
    //        if (!methods.ContainsKey(method))
    //            methods.Add(method, type.GetMethod(method));

            
    //        return methods[method];
    //    }

    //    public PropertyInfo GetProperty(string property)
    //    {
    //        if (!properties.ContainsKey(property))
    //            properties.Add(property, type.GetProperty(property));

    //        return allProperties[property];

    //    }
    //}
}
