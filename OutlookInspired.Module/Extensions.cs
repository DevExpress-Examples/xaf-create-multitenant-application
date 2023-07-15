using System.Collections.Concurrent;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors.Controls;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module{
    public static class Extensions {
        public static T FindObject<T>(this IObjectSpace objectSpace, Expression<Func<T,bool>> expression,bool inTransaction=false) 
            => objectSpace.GetObjectsQuery<T>(inTransaction).FirstOrDefault(expression);
        
        public static void SaveToFile(this Stream stream, string filePath) {
            var directory = Path.GetDirectoryName(filePath) + "";
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            using var fileStream = File.OpenWrite(filePath);
            stream.CopyTo(fileStream);
        }
        public static bool Any<T>(this IObjectSpace objectSpace) 
            => objectSpace.GetObjectsQuery<T>().Any();

        public static Stream GetManifestResourceStream(this Assembly assembly, Func<string, bool> nameMatch)
            => assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(nameMatch));
        public static Picture FromImage(this Image image) 
            => image == null ? null : new Picture{
                Data = ByteImageConverter.ToByteArray(image, image.RawFormat)
            };

        public static Image CreateImage(this byte[] data){
            if(data == null)
                throw new NotImplementedException();
            // return ResourceImageHelper.CreateImageFromResourcesEx("DevExpress.DevAV.Resources.Unknown-user.png", typeof(Employee).Assembly);
            return ByteImageConverter.FromByteArray(data);
        }

        public static IEnumerable<TValue> To<TSource,TValue>(this IEnumerable<TSource> source,TValue value) 
            => source.Select(_ => value);

        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source) => source.Select(item => (TResult)item);

        public static async IAsyncEnumerable<TResult> Select<TSource, TResult>(
            this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector){
            await foreach (var item in source){
                yield return selector(item);
            }
        }
        public static IAsyncEnumerable<object> Concat(this IAsyncEnumerable<object> first, IAsyncEnumerable<object> second) 
            => first.EnumerateAll( second);

        private static async IAsyncEnumerable<TSource> EnumerateAll<TSource>(this IAsyncEnumerable<TSource> first,
            IAsyncEnumerable<TSource> second){
            await foreach (var element in first.Enumerate()) yield return element;
            await foreach (var element in second.Enumerate()) yield return element;
        }

        private static async IAsyncEnumerable<TSource> Enumerate<TSource>(this IAsyncEnumerable<TSource> source){
            await foreach (var element in source) yield return element;
        }

        public static class AsyncEnumerable{
            public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator) => new DelegateAsyncEnumerable<T>(getEnumerator);

            private class DelegateAsyncEnumerable<T> : IAsyncEnumerable<T>{
                private readonly Func<CancellationToken, IAsyncEnumerator<T>> _getEnumerator;
                public DelegateAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> getEnumerator) => _getEnumerator = getEnumerator;
                public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => _getEnumerator(cancellationToken);
            }
        }

        public static async Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> source){
            var bag = new ConcurrentBag<T>();
            await foreach (var item in source){
                bag.Add(item);
            }
            return bag.ToArray();
        }
        public static async IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(
            this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector){
            await foreach (var item in source){
                yield return await selector(item);
            }
        }
        public static IEnumerable<TSource> Do<TSource>(
            this IEnumerable<TSource> source, Action<TSource> action)
            => source.Select(item => {
                action(item);
                return item;
            });
        
        public static IEnumerable<T> IgnoreElements<T>(this IEnumerable<T> source){
            foreach (var unused in source){
                yield break;
            }
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source,params T[] values){
            return source.Concat(values.AsEnumerable());
        }
        public static Dictionary<long, TObject> ToDictionary<TObject>(this IEnumerable<TObject> objects) where TObject:MigrationBaseObject
            => objects.ToDictionary(o => o.IdInt64,o => o);

    }
}