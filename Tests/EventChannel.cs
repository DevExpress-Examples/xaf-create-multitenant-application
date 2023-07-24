using System.Threading.Channels;

namespace Tests{
    public class EventChannel<T> : IDisposable{
        private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
        public Task Completion => _channel.Reader.Completion;
        public bool Post(T item) => _channel.Writer.TryWrite(item);

        public IAsyncEnumerable<T> ToAsyncEnumerable(CancellationToken token) => _channel.Reader.ReadAllAsync(token);
        internal readonly struct EventSubscription<TEventHandler> : IDisposable
            where TEventHandler : Delegate{
            private readonly Action _unsubscribe;
            public EventSubscription(
                TEventHandler handler,
                Action<TEventHandler> subscribe,
                Action<TEventHandler> unsubscribe){
                subscribe(handler);
                _unsubscribe = () => unsubscribe(handler);
            }

            public void Dispose() => _unsubscribe();
        }
        
        public IDisposable Subscribe<TEventHandler>(TEventHandler handler, Action<TEventHandler> subscribe,
            Action<TEventHandler> unsubscribe) where TEventHandler : Delegate 
            => new EventSubscription<TEventHandler>(handler, subscribe, unsubscribe);

        public void Dispose() => _channel.Writer.Complete();
    }
}