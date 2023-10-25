using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace XAF.Testing.RX{
    public static class UtilityExtensions{
        public static void OnNext<T>(this ISubject<T> subject) => subject.OnNext(default);
        public static T PushNext<T>(this ISubject<T> subject,T value){
            subject.OnNext(value);
            return value;
        }

        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,int seconds=1,bool delayOnEmpty=false) 
            => source.DelayOnContext(seconds.Seconds(),delayOnEmpty);
        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,TimeSpan? timeSpan,bool delayOnEmpty=false) 
            => source.If(_ => timeSpan.HasValue,arg => arg.DelayOnContext( (TimeSpan)timeSpan!),arg => arg.Observe())
                .SwitchIfEmpty(timeSpan.Observe().Where(_ => delayOnEmpty).WhenNotDefault().SelectMany(span => span.DelayOnContext((TimeSpan)span!)
                    .Select(_ => default(T)).IgnoreElements()));

        private static IObservable<T> DelayOnContext<T>(this T arg,TimeSpan timeSpan) 
            => arg.Observe()
                .SelectManySequential( arg1 => Observable.Return(arg1).Delay(timeSpan).ObserveOnContext())
                // .Delay(timeSpan, new SynchronizationContextScheduler(SynchronizationContext.Current!))
        ;

        public static IObservable<T> DelayOnContext<T>(this IObservable<T> source,TimeSpan timeSpan) 
            => source.DelayOnContext((TimeSpan?)timeSpan);
        public static IObservable<T> SubscribeReplay<T>(this IObservable<T> source, int bufferSize = 0){
            var replay = bufferSize > 0 ? source.Replay(bufferSize) : source.Replay();
            replay.Connect();
            return replay;
        }

        public static IObservable<T> Log<T>(this IObservable<T> source,Func<T,string> messageFactory,[CallerMemberName]string caller="") 
            => source.Do(x => $"{caller}: {messageFactory(x)}".LogValue());

        public static T LogValue<T>(this T value){
            Console.WriteLine(value);
            return value;
        }

        public static IObservable<T> ReplayConnect<T>(this IObservable<T> source, int bufferSize = 0) 
            => source.SubscribeReplay(bufferSize);
        public static Task Delay(this TimeSpan timeSpan,CancellationToken token=default) 
            => Task.Delay(timeSpan, token);
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
        
        public static IObservable<TSource> DoOnFirst<TSource>(this IObservable<TSource> source, Action<TSource> action)
            => source.DoWhen((i, _) => i == 0, action);
        public static IObservable<TSource> DoWhen<TSource>(this IObservable<TSource> source, Func<int,TSource, bool> predicate, Action<TSource> action)
            => source.Select((source1, i) => {
                if (predicate(i,source1)) {
                    action(source1);
                }
                return source1;
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
        
        public static IObservable<T> DoOnError<T>(this IObservable<T> source, Action<Exception> onError) 
            => source.Do(_ => { }, onError);
        
        public static TimeSpan TimeoutInterval = (Debugger.IsAttached ? 120 : 15).Seconds();
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
            return source.Log(messageFactory, caller).ThrowIfEmpty(timeoutMessage).Timeout(timeout ?? TimeoutInterval, timeoutMessage)
                .DelayOnContext(DelayOnContextInterval)
                .ReplayFirstTake();
        }
        
        public static IObservable<T> FinallySafe<T>(this IObservable<T> source, Action finallyAction,[CallerMemberName]string caller="" ) 
            => Observable.Create<T>(observer => {
                var finallyOnce = Disposable.Create(finallyAction);
                var subscription = source.Subscribe(observer.OnNext, error => {
                    try {
                        finallyOnce.Dispose();
                    }
                    catch (Exception ex) {
                        observer.OnError(ex);
                        return;
                    }

                    observer.OnError(error);
                }, () => {
                    try {
                        finallyOnce.Dispose();
                    }
                    catch (Exception ex) {
                        ex.Source = caller;
                        observer.OnError(ex);
                        return;
                    }

                    observer.OnCompleted();
                });
                return new CompositeDisposable(subscription, finallyOnce);
            });
        

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
