using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using DevExpress.Persistent.Base;
using Microsoft.JSInterop;

namespace XAF.Testing.XAF{
    public static class TracingExtensions{
        public static IObservable<T> LogError<T>(this IObservable<T> source) 
            => source.Publish(obs => Tracing.WhenError().ThrowTestException().To<T>().Merge(obs).TakeUntilCompleted(obs))
                .DoOnError(exception => DevExpress.Persistent.Base.Tracing.Tracer.LogError(exception))
            ;
    }
    public class Tracing:DevExpress.Persistent.Base.Tracing{
        readonly Subject<Exception> _errorSubject = new();
        
        public static IObservable<Exception> WhenError([CallerMemberName]string caller="") 
            => ((Tracing)Tracer)._errorSubject.Where(exception => exception is not JSDisconnectedException).Select(exception => exception.ToTestException(caller)).AsObservable();

        public static IObservable<Tracing> Use() 
            => typeof(Tracing).WhenEvent<CreateCustomTracerEventArgs>(nameof(CreateCustomTracer))
                .Select(e => e.Tracer = new Tracing()).Take(1).Cast<Tracing>()
                .Merge(Unit.Default.DeferAction(_ => Initialize()).IgnoreElements().To<Tracing>());

        public override void LogError(Exception exception){
            base.LogError(exception);
            Notify(exception);
        }

        public override void LogVerboseError(Exception exception){
            base.LogVerboseError(exception);
            Notify(exception);
        }
            
        private void Notify(Exception exception) => _errorSubject.OnNext(exception);
    }
}