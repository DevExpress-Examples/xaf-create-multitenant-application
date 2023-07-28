using System.Reflection;
using System.Runtime.ExceptionServices;

namespace OutlookInspired.Module.Services{
    internal static class ReflectionExtensions{
        public static bool IsPublic(this MemberInfo memberInfo) 
            => memberInfo switch {
                FieldInfo fieldInfo => fieldInfo.IsPublic,
                PropertyInfo propertyInfo => propertyInfo.GetGetMethod()?.IsPublic == true || propertyInfo.GetSetMethod()?.IsPublic == true,
                MethodInfo methodInfo => methodInfo.IsPublic,
                EventInfo eventInfo => eventInfo.GetAddMethod()?.IsPublic == true || eventInfo.GetRemoveMethod()?.IsPublic == true,
                _ => false
            };
        
        public static IOrderedEnumerable<MemberInfo> GetMembers(this Type type, MemberTypes memberType,  BindingFlags? flags=null) 
            => type.GetMembers(flags??BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(member => member.MemberType == memberType)
                .OrderByDescending(member => (member is MethodInfo{ IsPublic: true }));
        
        public static T CreateInstance<T>(this Type type) => (T)CreateInstance(type);

        public static object CreateInstance(this Type type){
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);
            throw new InvalidOperationException($"Type {type.FullName} does not have a parameterless constructor.");
        }
        
        public static bool IsDefaultValue<TSource>(this TSource source) {
            var def = default(TSource);
            return def != null ? def.Equals(source) : typeof(TSource) == typeof(object)
                ? source == null || source.Equals(source.GetType().DefaultValue()) : source == null;
        }

        public static bool IsDefaultValue(this object source) 
            => source == null || source.Equals(source.GetType().DefaultValue());
		
        public static bool IsDefaultValue(this object source,Type objectType) 
            => objectType.DefaultValue()==source;
        
        public static object DefaultValue(this Type t) => t.IsValueType ? Activator.CreateInstance(t) : null;
		
        public static T DefaultValue<T>(this Type t) => t.IsValueType||t.IsArray ? t.CreateInstance<T>() : default;
        
        public static IEnumerable<TSource> YieldItem<TSource>(this TSource source){
            yield return source;
        }
        public static object GetPropertyValue(this object obj, string propertyName) 
            => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(obj);
        
        public static void SetPropertyValue(this object obj, string propertyName, object value) 
            => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(obj, value);
    }
}