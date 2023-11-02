using System.Reactive.Linq;

namespace XAF.Testing.RX{
    public static class ErrorHandlingExtensions{
        public static IObservable<T> CompleteOnError<T>(this IObservable<T> source,Action<Exception> onError=null,Func<Exception,bool> match=null)
            => source.Catch<T,Exception>(exception => {
                if (!(match?.Invoke(exception) ?? true)) return exception.Throw<T>();
                onError?.Invoke(exception);
                return Observable.Empty<T>();
            });
    }
}