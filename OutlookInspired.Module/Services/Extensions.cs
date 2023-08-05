using System.Drawing;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.XtraEditors.Controls;

namespace OutlookInspired.Module.Services{
    public static class Extensions {
        public static decimal RoundNumber(this decimal d, int decimals = 0) 
            => Math.Round(d, decimals);
        
        public static byte[] ImageBytes(this Enum @enum) 
            => ImageLoader.Instance.GetEnumValueImageInfo(@enum).ImageBytes;
        
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
        public static Image CreateImage(this byte[] data){
            if(data == null)
                throw new NotImplementedException();
            // return ResourceImageHelper.CreateImageFromResourcesEx("DevExpress.DevAV.Resources.Unknown-user.png", typeof(Employee).Assembly);
            return ByteImageConverter.FromByteArray(data);
        }
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
        
    }
    
    public struct EditorAliases {
        public const string PdfViewerEditor = "PdfViewerEditor";
        public const string LabelPropertyEditor = "LabelPropertyEditor";
        public const string HyperLinkPropertyEditor = "HyperLinkPropertyEditor";
        public const string ProgressEditor = "ProgressEditor";
        
    }

}