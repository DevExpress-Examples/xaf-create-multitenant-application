using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OutlookInspired.Module.Services.Internal{
    internal static class Extensions{
        public static Color ColorFromHex(this string hex)
        {
            hex = hex.Replace("#", "");
            return Color.FromArgb(hex.Substring(0, 2).ToByte( 16), hex.Substring(2, 2).ToByte( 16), hex.Substring(4, 2).ToByte(16));
        }
        public static byte ToByte(this string value,int fromBase) 
            => Convert.ToByte(value, fromBase);

        public static T DeSerialize<T>(this string value)
            => JsonSerializer.Deserialize<T>(value);
        public static T DeSerialize<T>(this JsonElement jsonElement)
            => jsonElement.GetRawText().DeSerialize<T>();
        
        public static string Serialize(this object value) 
            => JsonConvert.SerializeObject(value);

        public static IMemberInfo FindDisplayableMember(this IMemberInfo memberInfo) 
            => ReflectionHelper.FindDisplayableMemberDescriptor(memberInfo);
        
        public static Type GetExpressionType(this LambdaExpression expression) 
            => expression.Parameters[0].Type;
        public static CriteriaOperator ToCriteria<T>(this Expression<Func<T, bool>> expression) 
            => CriteriaOperator.FromLambda(expression);
        public static CriteriaOperator Combine(this CriteriaOperator criteriaOperator,string criteria,GroupOperatorType type=GroupOperatorType.And){
            var @operator = CriteriaOperator.Parse(criteria);
            return !criteriaOperator.ReferenceEquals(null) ? new GroupOperator(type, @operator, criteriaOperator) : @operator;
        }

        public static string ToBase64String(this byte[] bytes) 
            => Convert.ToBase64String(bytes);
        public static byte[] ToBase64String(this string base64String) 
            => Convert.FromBase64String(base64String);

        public static string ToBase64Image(this byte[] bytes) 
            => $"data:{bytes.FileType()};base64,{bytes?.ToBase64String()}";

        private static bool IsMaskMatch(this byte[] byteArray, int offset, params byte[] mask) 
            => byteArray != null && byteArray.Length >= offset + mask.Length &&
               !mask.Where((t, i) => byteArray[offset + i] != t).Any();

        public static string FileType(this byte[] value) 
            => value switch{
                { Length: > 0 } when value.IsMaskMatch( 0, 77, 77) || value.IsMaskMatch(0, 73, 73) => "tiff",
                { Length: > 0 } when value.IsMaskMatch(1, 80, 78, 71) => "png",
                { Length: > 0 } when value.IsMaskMatch(0, 71, 73, 70, 56) => "gif",
                { Length: > 0 } when value.IsMaskMatch( 0, 255, 216) => "jpeg",
                { Length: > 0 } when value.IsMaskMatch(0, 66, 77) => "bmp",
                _ => ""
            };


        public static string GetString(this byte[] bytes, Encoding encoding = null) 
            => bytes == null ? null : (encoding ?? Encoding.UTF8).GetString(bytes);
        
        public static byte[] Bytes(this Stream stream){
            if (stream is MemoryStream memoryStream){
                return memoryStream.ToArray();
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public static decimal RoundNumber(this decimal d, int decimals = 0) 
            => Math.Round(d, decimals);
        
        public static ImageInfo ImageInfo(this Enum @enum) 
            => ImageLoader.Instance.GetEnumValueImageInfo(@enum);
        public static string ImageName(this Enum @enum) 
            => ImageLoader.Instance.GetEnumValueImageName(@enum);
        
        public static void SaveToFile(this Stream stream, string filePath) {
            var directory = Path.GetDirectoryName(filePath) + "";
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            using var fileStream = File.OpenWrite(filePath);
            stream.CopyTo(fileStream);
        }

        public static Stream GetManifestResourceStream(this Assembly assembly, Func<string, bool> nameMatch)
            => assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(nameMatch));

        public static IEnumerable<(TAttribute attribute,IMemberInfo memberInfo)> AttributedMembers<TAttribute>(this ITypeInfo info)  
            => info.Members.SelectMany(memberInfo => memberInfo.FindAttributes<Attribute>().OfType<TAttribute>().Select(attribute => (attribute, memberInfo)));
        public static Type GetAssemblyType(this AppDomain domain, string fullName,bool ignoreCase=false) 
            => fullName==null?null:domain.GetAssemblies().Select(assembly => assembly.GetType(fullName,ignoreCase)).WhereNotDefault().FirstOrDefault();
        
        public static Task AsTask(this CancellationToken cancellationToken){
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false);
            return tcs.Task;
        }

        public static IEnumerable<T> ToEnumerable<T>(this T obj){
            yield return obj;
        }
        
        
        public static Task DelayAndExit(this XafApplication application,TimeSpan delay) => Task.Delay(delay).ContinueWith(_ => application.Exit());

        public static CancellationToken AsCancelable(this Task task){
            var cts = new CancellationTokenSource();
            task.ContinueWith(_ => cts.Cancel(), TaskScheduler.Default);
            return cts.Token;
        }
    
        public static string MemberExpressionName<TObject>(this Expression<Func<TObject, object>> memberName) 
            => memberName.Body is UnaryExpression unaryExpression
                ? ((MemberExpression) unaryExpression.Operand).Member.Name
                : ((MemberExpression) memberName.Body).Member.Name;
        
        public static string MemberExpressionName<TObject,TMemberValue>(this Expression<Func<TObject, TMemberValue>> memberName) 
            => memberName.Body is UnaryExpression unaryExpression
                ? ((MemberExpression) unaryExpression.Operand).Member.Name
                : ((MemberExpression) memberName.Body).Member.Name;
    }
    
    public struct EditorAliases {
        
        public const string PrintLayoutRichTextEditor = "PrintLayoutRichTextEditor";
        public const string PdfViewerEditor = "PdfViewerEditor";
        public const string LabelPropertyEditor = "LabelPropertyEditor";
        public const string HyperLinkPropertyEditor = "HyperLinkPropertyEditor";
        public const string ProgressEditor = "ProgressEditor";
        
    }

}