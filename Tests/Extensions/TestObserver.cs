namespace Tests.Extensions{
    public class TestObserver<T> : IAsyncEnumerator<T>{
        private readonly IAsyncEnumerable<T> _asyncEnumerable;
        private IAsyncEnumerator<T>? _asyncEnumerator;
        public List<T> Items { get; }

        public int ItemsCount => Items.Count;
        public TestObserver(IAsyncEnumerable<T> asyncEnumerable){
            _asyncEnumerable = asyncEnumerable;
            Items = new List<T>();
        }

        public ValueTask DisposeAsync() => _asyncEnumerator?.DisposeAsync() ?? ValueTask.CompletedTask;
        public ValueTask<bool> MoveNextAsync() => _asyncEnumerator?.MoveNextAsync() ?? ValueTask.FromException<bool>(EnumeratorNullReferenceException);
        public T Current => _asyncEnumerator != null ? _asyncEnumerator.Current : throw EnumeratorNullReferenceException;
        private NullReferenceException EnumeratorNullReferenceException => new(nameof(_asyncEnumerator));
        public async Task EnumerateAsync(){
            _asyncEnumerator = _asyncEnumerable.GetAsyncEnumerator();
            try{
                while (await _asyncEnumerator.MoveNextAsync()){
                    Items.Add(_asyncEnumerator.Current);
                    Console.WriteLine("Added item to Items: " + _asyncEnumerator.Current);
                }

                ;
            }
            finally{
                await _asyncEnumerator.DisposeAsync();
            }
        }
    }
}