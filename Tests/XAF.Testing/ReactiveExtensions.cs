using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XAF.Testing{
    public static class ReactiveExtensions{
        private static readonly ConcurrentDictionary<(Type type, string eventName),(EventInfo info,MethodInfo add,MethodInfo remove)> Events = new();
        public static readonly IScheduler ImmediateScheduler=Scheduler.Immediate;
        
        public static IObservable<IList<T>> BufferUntilInactive<T>(this IObservable<T> source, TimeSpan delay,IScheduler scheduler=null)
            => source.BufferUntilInactive(delay,window => window.ToList(),scheduler);
        
        public static IObservable<IList<T>> BufferUntilInactive<T>(this IObservable<T> source, TimeSpan delay,Func<IObservable<T>,IObservable<IList<T>>> resultSelector,IScheduler scheduler=null)
            => source.Publish(obs => obs.Window(() => obs.Throttle(delay,scheduler??Scheduler.Default)).SelectMany(resultSelector));
        
        public static IObservable<TSource[]> BufferUntilCompleted<TSource>(this IObservable<TSource> source,bool skipEmpty=false) 
            => source.Buffer(Observable.Never<Unit>()).Where(sources => !skipEmpty || sources.Any()).Select(list => list.ToArray());

        public static IObservable<T> SwitchIfEmpty<T>(this IObservable<T> source, IObservable<T> switchTo) 
            => new AsyncSubject<Unit>().Use(signal => source.Do(_ => {
                signal.OnNext(Unit.Default); 
                signal.OnCompleted();
            }).Concat(switchTo.TakeUntil(signal)));
        
        public static IObservable<Unit> MergeToUnit<TSource, TValue>(this IObservable<TSource> source, IObservable<TValue> value) 
            => source.ToUnit().Merge(value.ToUnit());
        
        public static IObservable<object> MergeToObject<TSource, TValue>(this IObservable<TSource> source, IObservable<TValue> value) where TValue:class 
            => source.Select(source1 => source1 as object).WhenNotDefault().Merge(value.To<TValue>());

        public static IConnectableObservable<T> TakeAndReplay<T>(this IObservable<T> source, int count)
            => source.Take(count).Replay(count);
        
        public static IObservable<T> TakeUntilCompleted<T,T2>(this IObservable<T> source, IObservable<T2> next)
            => source.TakeUntil(next.WhenCompleted());
        public static IObservable<T> TakeUntilFinished<T,T2>(this IObservable<T> source, IObservable<T2> next)
            => source.TakeUntil(next.WhenFinished());
        
        public static IObservable<TResult> If<TSource, TResult>(this IObservable<TSource> source,
            Func<TSource, bool> predicate, Func<TSource, IObservable<TResult>> thenSource, Func<TSource, IObservable<TResult>> elseSource) 
            => source.SelectMany(value => predicate(value) ? thenSource(value) : elseSource(value));

        public static IObservable<EventPattern<object>> WhenEvent(this object source,string eventName,[CallerMemberName]string caller="") 
            => source.FromEventPattern<EventArgs>(eventName,caller)
                .Select(pattern => new EventPattern<object>(pattern.Sender, pattern.EventArgs));

        private static IObservable<EventPattern<TArgs>> FromEventPattern<TArgs>(this object source, string eventName,[CallerMemberName]string caller="") {
            var eventInfo = source.EventInfo(eventName);
            if ((eventInfo.info.EventHandlerType?.IsGenericType ?? false)&&eventInfo.info.EventHandlerType.GenericTypeArguments.First()==typeof(TArgs)) {
                return Observable.FromEventPattern<TArgs>(
                        handler => eventInfo.add.Invoke(source, new object[] { handler }),
                        handler => eventInfo.remove.Invoke(source, new object[] { handler }),ImmediateScheduler)
                    .Select(pattern => new EventPattern<TArgs>(pattern.Sender, pattern.EventArgs))
                    .TakeUntilDisposed(source as IComponent,caller);
            }

            if (eventInfo.add is{ IsPublic: true, IsStatic: false }) {
                return Observable.FromEventPattern<TArgs>(source, eventName,ImmediateScheduler)
                    .TakeUntilDisposed(source as IComponent,caller)
                    ;    
            }

            if (eventInfo.info.EventHandlerType == typeof(EventHandler)) {
                return Observable.FromEventPattern(
                        handler => eventInfo.add.Invoke(source, new object[] { handler }),
                        handler => eventInfo.remove.Invoke(source, new object[] { handler }),ImmediateScheduler)
                    .Select(pattern => new EventPattern<TArgs>(pattern.Sender, (TArgs)pattern.EventArgs))
                    .TakeUntilDisposed(source as IComponent,caller)
                    ;    
            }
            return Observable.FromEventPattern<TArgs>(
                    handler => eventInfo.add.Invoke(source, new object[] { handler }),
                    handler => eventInfo.remove.Invoke(source, new object[] { handler }),ImmediateScheduler)
                .Select(pattern => new EventPattern<TArgs>(pattern.Sender, pattern.EventArgs))
                .TakeUntilDisposed(source as IComponent,caller)
                ;
        }
        
        private static (EventInfo info,MethodInfo add,MethodInfo remove) EventInfo(this object source,string eventName) 
            => Events.GetOrAdd((source as Type ?? source.GetType(), eventName), t => {
                var eventInfo = (EventInfo)t.type.GetMembers(MemberTypes.Event,BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.FlattenHierarchy)
                    .OrderByDescending(info => info.IsPublic()).First(info => info.Name == eventName || info.Name.EndsWith($".{eventName}"));
                return (eventInfo, eventInfo.AddMethod,eventInfo.RemoveMethod)!;
            });
        
        public static IObservable<TEventArgs> WhenEvent<TEventArgs>(this object source, string eventName,[CallerMemberName]string caller="") 
            => source.FromEventPattern<TEventArgs>(eventName,caller).Select(pattern => pattern.EventArgs);
        
        public static IObservable<T> TakeUntilDisposed<T>(this IObservable<T> source, IComponent component, [CallerMemberName] string caller = "")
            => component != null ? source.TakeUntil(component.WhenDisposed()) : source;
        
        public static IObservable<TDisposable> WhenDisposed<TDisposable>(this TDisposable source) where TDisposable : IComponent 
            => Observable.FromEventPattern(source, nameof(source.Disposed), ImmediateScheduler).Take(1).To(source);

        public static IObservable<T> TakeOrOriginal<T>(this IObservable<T> source, int count) => count == 0 ? source : source.Take(count);

        public static IObservable<TSource> WhenDefault<TSource>(this IObservable<TSource> source) 
            => source.Where(obj => obj.IsDefaultValue());

        public static IObservable<TSource> WhenDefault<TSource,TValue>(this IObservable<TSource> source,Func<TSource, TValue> valueSelector) 
            =>source.Where(source1 => valueSelector(source1).IsDefaultValue());
        
        public static IObservable<TSource> WhenNotDefault<TSource,TValue>(this IObservable<TSource> source,Func<TSource,TValue> valueSelector) 
            =>source.Where(source1 => !ReflectionExtensions.IsDefaultValue(valueSelector(source1)));

        public static IObservable<TSource> WhenNotDefault<TSource>(this IObservable<TSource> source) => source.Where(s => !s.IsDefaultValue());
        
        public static IObservable<TSource> TakeWhileInclusive<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate) 
            => source.TakeUntil(source.SkipWhile(predicate).Skip(1));
        public static IObservable<TTarget> ConcatIgnoredValue<TSource,TTarget>(this IObservable<TSource> source, TTarget value) 
            => source.Select(_ => default(TTarget)).WhenNotDefault().Concat(value.Observe());
        
        public static IObservable<T> ConcatIgnored<T,T2>(this IObservable<T> source,Func<T,IObservable<T2>> secondSelector,Func<T,bool> merge=null)
            => source.SelectMany(arg => {
                merge ??= _ => true;
                return merge(arg) ? secondSelector(arg).IgnoreElements().ConcatIgnoredValue(arg).Finally(() => {}) : arg.Observe();
            });
        public static IObservable<T2> SelectManySequential<T1, T2>(this IObservable<T1> source, Func<T1, IObservable<T2>> selector) 
            => source.Select(x => Observable.Defer(() => selector(x))).Concat();
        
        
        public static IObservable<T> ThrowIfEmpty<T>(this IObservable<T> source,[CallerMemberName]string caller="")
            => source.SwitchIfEmpty(Observable.Defer(() => Observable.Throw<T>(new SequenceIsEmptyException($"source is empty {caller}"))));
        public static IObservable<T> ConcatDefer<T>(this IObservable<T> source, Func<IObservable<T>> target)
            => source.Concat(Observable.Defer(target));
        
        public static IObservable<T[]> WhenCompleted<T>(this IObservable<T> source) 
            => source.When(NotificationKind.OnCompleted).Select(_ => Array.Empty<T>());
        public static IObservable<Exception> WhenError<T>(this IObservable<T> source) 
            => source.When(NotificationKind.OnError).Select(notification => notification.Exception);
        public static IObservable<T[]> WhenFinished<T>(this IObservable<T> source) 
            => source.Publish(obs => obs.WhenCompleted().Merge(obs.WhenError().Select(_ => Array.Empty<T>())).Take(1));
        
        public static IObservable<Notification<T>> When<T>(this IObservable<T> source,NotificationKind notificationKind) 
            => source.Materialize().Where(notification => notification.Kind==notificationKind);

        public static IObservable<T> Observe<T>(this T self, IScheduler scheduler = null) 
            => Observable.Return(self, scheduler??ImmediateScheduler);

        public static IObservable<TValue> To<TSource,TValue>(this IObservable<TSource> source,TValue value) 
            => source.Select(_ => value);

        public static IObservable<T> To<T>(this IObservable<object> source) 
            => source.Select(o =>o is T arg? arg: default);
        
        public static IObservable<T> To<T>(this IObservable<Unit> source) 
            => source.Select(_ => default(T));
        
        public static IObservable<T1> ToFirst<T1, T2>(this IObservable<(T1, T2)> source) 
            => source.Select(tuple => tuple.Item1);

        public static IObservable<T2> ToSecond<T1, T2>(this IObservable<(T1, T2)> source) 
            => source.Select(tuple => tuple.Item2);
        
        public static IObservable<(TValue value, TSource source)> InversePair<TSource, TValue>(this IObservable<TSource> source, TValue value) 
            => source.Select(x => ( value,x));

        public static IObservable<Unit> ToUnit<T>(this IObservable<T> source) 
            => source.Select(_ => Unit.Default);
        public static IEnumerable<Unit> ToUnit<T>(this IEnumerable<T> source)
            => source.Select(_ => Unit.Default);
        
        public static IObservable<Unit> ToUnit<T1, T2>(this IObservable<(T1, T2)> source) 
            => source.Select(_ => Unit.Default);

        public static IObservable<T> WaitUntilInactive<T>(this IObservable<T> source, TimeSpan timeSpan,int count =1,IScheduler scheduler=null) 
            =>timeSpan==TimeSpan.Zero?source: source.BufferUntilInactive(timeSpan,scheduler:scheduler).SelectMany(list => list.TakeLast(count).ToArray());
        
        public static IObservable<T> WaitUntilInactive<T>(this IObservable<T> source, int seconds, int count = 1,IScheduler scheduler=null)
            => source.WaitUntilInactive(TimeSpan.FromSeconds(seconds), count,scheduler);
        
        public static IObservable<TSource> SelectMany<TSource>(this IObservable<IEnumerable<TSource>> source) 
            => source.SelectMany(source1 => source1.ToNowObservable());
        
        public static IObservable<TSource> ToNowObservable<TSource>(this IEnumerable<TSource> source)
            => source.ToObservable(ImmediateScheduler);
        
        public static IObservable<T> DoNotComplete<T>(this IObservable<T> source) => source.Concat(Observable.Never<T>());

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
                .SelectManySequential( arg1 => Observable.Return(arg1).Delay(timeSpan).ObserveOnContext());

        public static IObservable<T> Log<T>(this IObservable<T> source,Func<T,string> messageFactory,[CallerMemberName]string caller="") 
            => source.Do(x => $"{caller}: {messageFactory(x)}".LogValue());

        public static T LogValue<T>(this T value){
            Console.WriteLine(value);
            return value;
        }

        public static IObservable<TResult> Use<T, TResult>(this T source, Func<T, IObservable<TResult>> selector) where T : IDisposable
            => Observable.Using(() => source, selector);
        
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

        public static IObservable<T> ObserveOnContext<T>(this IObservable<T> source) {
            var synchronizationContext = SynchronizationContext.Current;
            return synchronizationContext != null ? source.ObserveOn(synchronizationContext) : source;
        }
        
        public static IObservable<TSource> DoWhen<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate, Action<TSource> action,Action<TSource> actionElse=null)
            => source.Do(source1 => {
                if (predicate(source1)) {
                    action(source1);
                }
                else{
                    actionElse?.Invoke(source1);
                }
            });
        
        public static IObservable<TSource> Timeout<TSource>(
            this IObservable<TSource> source, TimeSpan dueTime, string timeoutMessage) 
            => source.Timeout(dueTime, Observable.Throw<TSource>(new TimeoutException(timeoutMessage)));
        
        public static IObservable<T> DoOnComplete<T>(this IObservable<T> source, Action onComplete)
            => source.Do(_ => { }, onComplete);
        
        public static IObservable<T> DoOnError<T>(this IObservable<T> source, Action<Exception> onError) 
            => source.Do(_ => { }, onError);
        
        public static IObservable<T> DoAlways<T>(this IObservable<T> source, Action always) 
            => source.Publish(obs => obs.DoOnError(_ => always())
                .Merge(obs.DoOnComplete(always)).Merge(obs.Do(_ => always()))
                .IgnoreElements().Merge(obs));
        
        public static TimeSpan TimeoutInterval = (Debugger.IsAttached ? 120 : 15).Seconds();
        

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

        public static string MessageFactory<TSource>(this Func<TSource, string> messageFactory, string caller) => $"{caller}: {messageFactory(default)}";

        public static IObservable<T> ReplayFirstTake<T>(this IObservable<T> source,ConnectionMode mode=ConnectionMode.AutoConnect){
            var takeAndReplay = source.TakeAndReplay(1);
            return mode==ConnectionMode.AutoConnect?takeAndReplay.AutoConnect():takeAndReplay.RefCount();
        }

    }
    
    public enum ConnectionMode{
        AutoConnect,
        RefCount
    }
    public class SequenceIsEmptyException:Exception {
        public SequenceIsEmptyException(string message) : base(message){
        }
    }

}

