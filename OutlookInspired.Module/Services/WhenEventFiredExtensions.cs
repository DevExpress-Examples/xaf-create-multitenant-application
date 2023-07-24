using System.Reflection;
using System.Threading.Channels;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.Services{
    public class ManualAsyncEnumerable<T> : IAsyncEnumerable<T>{
        private readonly Func<CancellationToken, IAsyncEnumerator<T>> _enumeratorFactory;
        private ManualAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> enumeratorFactory) 
            => _enumeratorFactory = enumeratorFactory;
        public static ManualAsyncEnumerable<T> CreateInstance(Func<CancellationToken, IAsyncEnumerator<T>> enumeratorFactory) 
            => new(enumeratorFactory);
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) 
            => _enumeratorFactory(cancellationToken);
    }

    public class DisposableAsyncEnumerable<TEventArgs> : IAsyncEnumerable<TEventArgs>, IAsyncDisposable{
        private readonly IAsyncEnumerable<TEventArgs> _enumerable;
        private readonly Action _disposeAction;

        public DisposableAsyncEnumerable(IAsyncEnumerable<TEventArgs> enumerable, Action disposeAction){
            _enumerable = enumerable;
            _disposeAction = disposeAction;
        }

        public IAsyncEnumerator<TEventArgs> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => _enumerable.GetAsyncEnumerator(cancellationToken);

        public ValueTask DisposeAsync(){
            _disposeAction();
            return new ValueTask(Task.CompletedTask);
        }
    }

    public static class WhenEventFiredExtensions{
        public static IAsyncEnumerable<T> WhenEventFired<T>(this T source, string eventName)
            => source!.WhenEventFired<EventArgs>(eventName).To(source);
        
        public static IAsyncEnumerable<TEventArgs> WhenEventFired<TEventArgs>(this object source, string eventName)
            where TEventArgs : EventArgs{
            var eventInfo = source.GetType().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (eventInfo == null){
                throw new ArgumentException($"The event '{eventName}' does not exist.", nameof(eventName));
            }
            Channel<TEventArgs> channel = Channel.CreateUnbounded<TEventArgs>();
            EventHandler<TEventArgs> handler = (_, e) => {
                channel.Writer.TryWrite(e);
                Tracing.Tracer.LogText(e.ToString());
            };
            var delegateHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, handler.Target, handler.Method);
            var addMethod = eventInfo.GetAddMethod(nonPublic: true);
            var removeMethod = eventInfo.GetRemoveMethod(nonPublic: true);
            if (addMethod!.IsPublic && removeMethod!.IsPublic){
                eventInfo.AddEventHandler(source, delegateHandler);
            }
            else{
                addMethod.Invoke(source, new object[] { delegateHandler });
            }

            return channel.Reader.ReadAllAsync(CancellationToken.None);
            // var manualAsyncEnumerable = ManualAsyncEnumerable<TEventArgs>.CreateInstance(cancellationToken => channel.Reader.ReadAllAsync(cancellationToken).GetAsyncEnumerator(cancellationToken));
            // return new DisposableAsyncEnumerable<TEventArgs>(manualAsyncEnumerable, () => {
            //     if (addMethod.IsPublic && removeMethod!.IsPublic){
            //         eventInfo.RemoveEventHandler(source, delegateHandler);
            //     }
            //     else{
            //         removeMethod!.Invoke(source, new object[] { delegateHandler });
            //     }
            // });
        }
        // public static IAsyncEnumerable<TEventArgs> WhenEventFired1<TEventArgs>(this object source, string eventName)
        //     where TEventArgs : EventArgs{
        //     var eventInfo = source.GetType().GetEvent(eventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        //     if (eventInfo == null){
        //         throw new ArgumentException($"The event '{eventName}' does not exist.", nameof(eventName));
        //     }
        //     var channel = Channel.CreateUnbounded<TEventArgs>();
        //     EventHandler<TEventArgs> handler = (_, e) => {
        //         channel.Writer.TryWrite(e);
        //     };
        //     var delegateHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, handler.Target, handler.Method);
        //     var addMethod = eventInfo.GetAddMethod(nonPublic: true);
        //     var removeMethod = eventInfo.GetRemoveMethod(nonPublic: true);
        //     if (addMethod!.IsPublic && removeMethod!.IsPublic){
        //         eventInfo.AddEventHandler(source, delegateHandler);
        //     }
        //     else{
        //         addMethod.Invoke(source, new object[] { delegateHandler });
        //     }
        //     var manualAsyncEnumerable = ManualAsyncEnumerable<TEventArgs>.CreateInstance(cancellationToken => channel.Reader.ReadAllAsync(cancellationToken).GetAsyncEnumerator(cancellationToken));
        //     return new DisposableAsyncEnumerable<TEventArgs>(manualAsyncEnumerable, () => {
        //         if (addMethod.IsPublic && removeMethod!.IsPublic){
        //             eventInfo.RemoveEventHandler(source, delegateHandler);
        //         }
        //         else{
        //             removeMethod!.Invoke(source, new object[] { delegateHandler });
        //         }
        //     });
        // }
    }
}