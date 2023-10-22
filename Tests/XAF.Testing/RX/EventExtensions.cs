using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XAF.Testing.RX{
    public static class EventExtensions{
        private static readonly ConcurrentDictionary<(Type type, string eventName),(EventInfo info,MethodInfo add,MethodInfo remove)> Events = new();
        public static readonly IScheduler ImmediateScheduler=Scheduler.Immediate;
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

        public static IObservable<(TEventArgs args, TSource source)> WhenEvent<TSource,TEventArgs>(this object source, string eventName,[CallerMemberName]string caller="")
            => source.FromEventPattern<TEventArgs>(eventName,caller).Select(pattern => (pattern.EventArgs,(TSource)source));
        
        public static IObservable<TEventArgs> WhenEvent<TEventArgs>(this object source, string eventName,[CallerMemberName]string caller="") 
            => source.FromEventPattern<TEventArgs>(eventName,caller).Select(pattern => pattern.EventArgs);
        
        public static IObservable<T> TakeUntilDisposed<T>(this IObservable<T> source, IComponent component, [CallerMemberName] string caller = "")
            => component != null ? source.TakeUntil(component.WhenDisposed(caller)) : source;
        
        public static IObservable<TDisposable> WhenDisposed<TDisposable>(this TDisposable source,[CallerMemberName]string caller="") where TDisposable : IComponent 
            => Observable.FromEventPattern(source, nameof(source.Disposed), ImmediateScheduler).Take(1).To(source);
    }
}