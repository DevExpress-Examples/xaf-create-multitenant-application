using System.Collections;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using DevExpress.Data;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.EFCore.Utils;
using XAF.Testing.RX;

namespace XAF.Testing{
    public static class ReflectionExtensions{
        public static string ToValidFileName(this string input) {
            var invalidChars = Path.GetInvalidFileNameChars();
            var validString = new string(input.Where(ch => !invalidChars.Contains(ch)).ToArray()).Replace(" ", "_");
            return Regex.Replace(validString.Length > 250 ? validString.Substring(0, 250) : validString, "[^a-zA-Z0-9_]", "");
        }
        public static bool HasFlags(this Enum flag,params Enum[] values) 
            => values.All(flag.HasFlag);

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
        public static Exception AddScreenshot(this Exception exception) 
            => new($"{exception.Message} {ScreenCapture.CaptureActiveWindowAndSave()}", exception);

        public static void ThrowCaptured(this Exception exception)
            =>exception.Capture().Throw();
        public static ExceptionDispatchInfo Capture(this Exception exception)
            =>ExceptionDispatchInfo.Capture(exception);

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

        [Obsolete("make it ObserveItems and remove the Take(1) after it")]
        public static IObservable<object> YieldItems(this object value,int count=0) 
            => value switch{
                null => Observable.Empty<object>(),
                EntityServerModeFrontEnd source => 0.Range(source.Count).Select(i => source[i]).Where(row => row != null).ToAsyncEnumerable().TakeOrOriginal(source.Count<count?source.Count:count).ToObservable(),
                ServerModeSourceAdderRemover source => source.GetType().GetFields(BindingFlags.NonPublic|BindingFlags.Instance)
                    .First(info => typeof(IListServer).IsAssignableFrom(info.FieldType)).GetValue(source).YieldItems(count),
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