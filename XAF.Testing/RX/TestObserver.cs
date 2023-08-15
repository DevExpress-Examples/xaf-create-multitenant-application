namespace XAF.Testing.RX{
    public class TestObserver<T> : IObserver<T>{
        
        private readonly List<T> _items;

        public TestObserver() => _items = new List<T>();

        public IReadOnlyList<T> Items => _items;

        public int ItemsCount => _items.Count;
        
        public Exception Error{ get; private set; }

        public bool Completed{ get; private set; }

        void IObserver<T>.OnNext(T value) => _items.Add(value);

        public void OnError(Exception error) => Error = error;

        void IObserver<T>.OnCompleted() => Completed = true;

    }
}