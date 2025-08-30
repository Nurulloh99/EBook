namespace EBook.Core.Helpers
{
    public class SequentialSpawner<T> : IDisposable
    {
        private List<T> Objects;
        private Object LockHandle = new Object();
        private Boolean _IsProcessing;
        private Action<T[]> Callback;
        public event Action<SequentialSpawner<T>> IsProcessingChanged;
        public event Action<SequentialSpawner<T>, SequentialSpawnerEventArgs<T>> OnCallbackCompleted;
        private Int32 Counter;
        public Boolean IsProcessing
        {
            get
            {
                return _IsProcessing;
            }
            set
            {
                if (_IsProcessing != value)
                {
                    _IsProcessing = value;
                    var handle = IsProcessingChanged;
                    handle.Try(x => x(this));
                }
            }
        }
        public Boolean HasObjectsToProcess
        {
            get
            {
                lock (LockHandle)
                    return Objects.Return(x => x.Any(), false);
            }
        }
        public SequentialSpawner(Action<T[]> callback)
        {
            Guard.ParamNotNull(callback, "callback");
            Callback = callback;
            Objects = new List<T>();
        }
        public void Add(T obj)
        {
            lock (LockHandle)
                if (Objects != null && Callback != null && IsProcessing)
                    Objects.Add(obj);
                else if (Objects != null && Callback != null)
                {
                    IsProcessing = true;
                    Callback.BeginInvoke(new[] { obj }, CallbackCompleted, null);

                }
        }
        public void AddRange(IEnumerable<T> objects)
        {
            Guard.ParamNotNull(objects, "objects");
            lock (LockHandle)
                if (Objects != null && Callback != null && IsProcessing)
                    Objects.AddRange(objects);
                else
                {
                    IsProcessing = true;
                    Callback.BeginInvoke(objects.ToArray(), CallbackCompleted, null);
                }
        }
        public void Dispose()
        {
            lock (LockHandle)
            {
                Objects.Clear();
                Objects = null;
                Callback = null;
            }
        }
        private void CallbackCompleted(IAsyncResult result)
        {
            Interlocked.Increment(ref Counter);
            lock (LockHandle)
                if (Objects != null && Callback != null && Objects.Any())
                {
                    var arr = Objects.ToArray();
                    Objects.Clear();
                    Callback.BeginInvoke(arr, CallbackCompleted, null);
                }
                else
                {
                    IsProcessing = false;
                }
        }
    }
}