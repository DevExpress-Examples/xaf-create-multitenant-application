using System.Collections;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using DevExpress.Data;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.EFCore.Utils;

namespace XAF.Testing{
    public static class ReflectionExtensions{

        public static Stream GetManifestResourceStream(this Assembly assembly, Func<string, bool> nameMatch)
            => assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(nameMatch));
        public static void Save(this byte[] bytes, string path) 
            => File.WriteAllBytes(path, bytes);
        public static byte[] Bytes(this Stream stream){
            if (stream is MemoryStream memoryStream) return memoryStream.ToArray();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();

        }

        public static void ThrowCaptured(this Exception exception)
            =>exception.Capture().Throw();
        public static ExceptionDispatchInfo Capture(this Exception exception)
            =>ExceptionDispatchInfo.Capture(exception);

        public static object GetPropertyValue(this object obj, string propertyName) 
            => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(obj);

        public static void SetPropertyValue(this object obj, string propertyName, object value) 
            => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(obj, value);
        
        public static IOrderedEnumerable<MemberInfo> GetMembers(this Type type, MemberTypes memberType,  BindingFlags? flags=null) 
            => type.GetMembers(flags??BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(member => member.MemberType == memberType)
                .OrderByDescending(member => (member is MethodInfo{ IsPublic: true }));

        public static bool IsDefaultValue<TSource>(this TSource source) {
            var def = default(TSource);
            return def != null ? def.Equals(source) : typeof(TSource) == typeof(object)
                ? source == null || source.Equals(source.GetType().DefaultValue()) : source == null;
        }
        
        public static MethodInfo GetStaticMethod(this Type type, string name) 
            => type.GetMethods().Where(info => info.Name==name)
                .First(info => info.IsStatic&&info.IsPublic
                        &&info.GetParameters().Length==2&& info.GetParameters().Last().ParameterType==typeof(int));

        
        public static IObservable<object> ObserveItems(this object value,int count=0) 
            => value switch{
                null => Observable.Empty<object>(),
                EntityServerModeFrontEnd source => 0.Range(source.Count).Select(i => source[i]).Where(row => row != null).ToAsyncEnumerable().TakeOrOriginal(source.Count<count?source.Count:count).ToObservable(),
                ServerModeSourceAdderRemover source => source.GetType().GetFields(BindingFlags.NonPublic|BindingFlags.Instance)
                    .First(info => typeof(IListServer).IsAssignableFrom(info.FieldType)).GetValue(source).ObserveItems(count),
                EFCoreServerCollection source=>source.QueryableSource.PaginateAsync(100>count?count:100).TakeOrOriginal(count).ToObservable(),
                IEnumerable source => source.Cast<object>().ToNowObservable(),
                _ => value.YieldItem().ToNowObservable()
            };


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