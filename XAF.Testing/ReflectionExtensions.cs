using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;

namespace XAF.Testing{
    public static class ReflectionExtensions{
        public static string ToValidFileName(this string input) {
            var invalidChars = Path.GetInvalidFileNameChars();
            var validString = new string(input.Where(ch => !invalidChars.Contains(ch)).ToArray()).Replace(" ", "_");
            return Regex.Replace(validString.Length > 250 ? validString.Substring(0, 250) : validString, "[^a-zA-Z0-9_]", "");
        }

        public static void ThrowCaptured(this Exception exception)
            =>ExceptionDispatchInfo.Capture(exception).Throw();

        public static IOrderedEnumerable<MemberInfo> GetMembers(this Type type, MemberTypes memberType,  BindingFlags? flags=null) 
            => type.GetMembers(flags??BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(member => member.MemberType == memberType)
                .OrderByDescending(member => (member is MethodInfo{ IsPublic: true }));

        public static bool IsDefaultValue<TSource>(this TSource source) {
            var def = default(TSource);
            return def != null ? def.Equals(source) : typeof(TSource) == typeof(object)
                ? source == null || source.Equals(source.GetType().DefaultValue()) : source == null;
        }
        
        public static IEnumerable<TSource> YieldItem<TSource>(this TSource source){
            yield return source;
        }

        public static bool IsPublic(this MemberInfo memberInfo) 
            => memberInfo switch {
                FieldInfo fieldInfo => fieldInfo.IsPublic,
                PropertyInfo propertyInfo => propertyInfo.GetGetMethod()?.IsPublic == true || propertyInfo.GetSetMethod()?.IsPublic == true,
                MethodInfo methodInfo => methodInfo.IsPublic,
                EventInfo eventInfo => eventInfo.GetAddMethod()?.IsPublic == true || eventInfo.GetRemoveMethod()?.IsPublic == true,
                _ => false
            };

    }
}