using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using XAF.Testing.XAF;

namespace XAF.Testing.RX{
    public static class UtilityExtensions{
        public static IObservable<T> SubscribeReplay<T>(this IObservable<T> source, int bufferSize = 0){
            var replay = bufferSize > 0 ? source.Replay(bufferSize) : source.Replay();
            replay.Connect();
            return replay;
        }

        public static IObservable<T> Log<T>(this IObservable<T> source,Func<T,string> messageFactory,[CallerMemberName]string caller="") 
            => source.Do(x => Console.WriteLine($"{caller}: {messageFactory(x)}"));

        public static IObservable<T> ReplayConnect<T>(this IObservable<T> source, int bufferSize = 0) 
            => source.SubscribeReplay(bufferSize);
        
        public static IObservable<TResult> Use<T, TResult>(this T source, Func<T, IObservable<TResult>> selector) where T : IDisposable
            => Observable.Using(() => source, selector);
        
        public static IObservable<T> Defer<T>(this object o, IObservable<T> execute)
            => Observable.Defer(() => execute);
        
        public static IObservable<Unit> DeferAction<T>(this T o, Action execute)
            => Observable.Defer(() => {
                execute();
                return Observable.Empty<Unit>();
            });
        public static IObservable<Unit> DeferAction<T>(this T o, Action<T> execute)
            => Observable.Defer(() => {
                execute(o);
                return Observable.Empty<Unit>();
            });

        public static IObservable<T> Defer<T,TObject>(this TObject o, Func<TObject,IObservable<T>> selector)
            => Observable.Defer(() => selector(o));
        
        public static IObservable<T> Defer<T>(this object o, Func<IObservable<T>> selector)
            => Observable.Defer(selector);
        
        public static IObservable<T> Defer<T>(this object o, Func<IEnumerable<T>> selector)
            => Observable.Defer(() => selector().ToNowObservable());
        
        public static IObservable<T> ObserveOnContext<T>(this IObservable<T> source, SynchronizationContext synchronizationContext) 
            => source.ObserveOn(synchronizationContext);

        public static IObservable<T> ObserveOnContext<T>(this IObservable<T> source, bool throwIfNull) 
            => source.If(_ => throwIfNull && SynchronizationContext.Current == null,
                () => Observable.Throw<T>(new NullReferenceException(nameof(SynchronizationContext))), () => source);

        public static IObservable<T> ObserveOnContext<T>(this IObservable<T> source) {
            var synchronizationContext = SynchronizationContext.Current;
            return synchronizationContext != null ? source.ObserveOn(synchronizationContext) : source;
        }
        
        public static IObservable<T> DoAfterSubscribe<T>(this IObservable<T> source, Action action) 
            => Observable.Create<T>(observer => {
                var disposable = source.Subscribe(observer);
                action();
                return disposable;
            });
        
        public static IObservable<TSource> DoWhen<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate, Action<TSource> action,Action<TSource> actionElse=null)
            => source.Do(source1 => {
                if (predicate(source1)) {
                    action(source1);
                }
                else{
                    actionElse?.Invoke(source1);
                }
            });

        private static int _nestingLevel;
        private static readonly TimeSpan BaseTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan MinimumTimeout = TimeSpan.FromSeconds(25);

        public static IObservable<T> LayerTimeout<T>(this IObservable<T> source, string timeoutMessage,[CallerMemberName]string caller=""){
            Interlocked.Increment(ref _nestingLevel);
            var timeout = TimeSpan.FromSeconds(Math.Max(BaseTimeout.TotalSeconds - _nestingLevel , MinimumTimeout.TotalSeconds));
            return source.Timeout(timeout,timeoutMessage).Finally(() => Interlocked.Decrement(ref _nestingLevel));
        }
        public static IObservable<TSource> Timeout<TSource>(
            this IObservable<TSource> source, TimeSpan dueTime, string timeoutMessage) 
            => source.Timeout(dueTime, Observable.Throw<TSource>(new TimeoutException(timeoutMessage)));
        
        public static IObservable<T> DoOnComplete<T>(this IObservable<T> source, Action onComplete)
            => source.Do(_ => { }, onComplete);
        
        public static TimeSpan TimeoutInterval = (Debugger.IsAttached ? 120 : 45).Seconds();
        public static IObservable<TSource> Timeout<TSource>(
            this IObservable<TSource> source,string message) 
            => source.Timeout(TimeoutInterval, Observable.Throw<TSource>(new TimeoutException(message)));

        public static IObservable<TSource> Assert<TSource>(
            this IObservable<TSource> source, TimeSpan? timeout = null, [CallerMemberName] string caller = "") 
            => source.Assert(_ => "",timeout,caller);

        public static IObservable<TSource> Assert<TSource>(this IObservable<TSource> source, string message, TimeSpan? timeout = null,[CallerMemberName]string caller="")
            => source.Assert(_ => message,timeout,caller);

        public static TimeSpan? DelayOnContextInterval=250.Milliseconds();
        public static IObservable<TSource> Assert<TSource>(this IObservable<TSource> source,Func<TSource,string> messageFactory,TimeSpan? timeout=null,[CallerMemberName]string caller=""){
            var timeoutMessage = messageFactory.MessageFactory(caller);
            return source.TakeAndReplay(1).RefCount()
                .DelayOnContext(DelayOnContextInterval)
                .Log(messageFactory, caller)
                .ThrowIfEmpty(timeoutMessage)
                .Timeout(timeout ?? TimeoutInterval, timeoutMessage);
        }

        public static IObservable<T> Throw<T>(this Exception exception) => Observable.Throw<T>(exception);
        public static string MessageFactory<TSource>(this Func<TSource, string> messageFactory, string caller) => $"{caller}: {messageFactory(default)}";

        public static IObservable<T> ReplayFirstTake<T>(this IObservable<T> source,ConnectionMode mode=ConnectionMode.AutoConnect){
            var takeAndReplay = source.TakeAndReplay(1);
            return mode==ConnectionMode.AutoConnect?takeAndReplay.AutoConnect():takeAndReplay.RefCount();
        }

        public static IObservable<T> AutoConnectAndReplayFirstTake<T>(this IObservable<T> source) 
            => source.ReplayFirstTake();

        public static TestObserver<T> Test<T>(this IObservable<T> source){
            var testObserver = new TestObserver<T>();
            source.Subscribe(testObserver);
            return testObserver;
        }

        public static IObservable<T> AsyncFinally<T>(this IObservable<T> source, Func<IObservable<object>> action)
            => source.AsyncFinally(async () => await action().ToTask());

        public static IObservable<T> AsyncFinally<T>(this IObservable<T> source, Func<Task> action) 
            => source
                .Materialize()
                .SelectMany(async n => {
                    switch (n.Kind){
                        case NotificationKind.OnCompleted:
                        case NotificationKind.OnError:
                            await action();
                            return n;
                        case NotificationKind.OnNext:
                            return n;
                        default:
                            throw new NotImplementedException();
                    }
                })
                .Dematerialize();
    }
    
    public enum ConnectionMode{
        AutoConnect,
        RefCount
    }

}
