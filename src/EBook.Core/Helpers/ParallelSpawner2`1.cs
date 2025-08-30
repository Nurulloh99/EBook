namespace EBook.Core.Helpers
{
    public class ParallelSpawner2<T>
    {
        private readonly Action<T> Callback;
        private readonly int DegreeOfParallelizm;
        private Queue<T> Objects;
        private List<Thread> Threads;
        private readonly object LockHandle = new object();
        public bool IsProcessing
        {
            get
            {
                lock (LockHandle)
                    return Threads.Count > 0;
            }
        }
        public event Action<ParallelSpawner2<T>> IsProcessingChanged;

        public ParallelSpawner2(Action<T> callback, int degreeOfParallelizm)
        {
            Guard.ParamNotNull(callback, nameof(callback));
            Guard.IsTrue<ArgumentException>(degreeOfParallelizm > 0);
            Callback = callback;
            DegreeOfParallelizm = degreeOfParallelizm;
            Threads = new List<Thread>();
            Objects = new Queue<T>();
        }

        public void Enqueue(T @object)
        {
            lock (LockHandle)
            {
                Objects.Enqueue(@object);
            }
            Check();
        }

        private void StartProcessing(T @object)
        {
            var t = new Thread(Run);
            var justAdded = false;
            lock (LockHandle)
            {
                Threads.Add(t);
                if (Threads.Count == 1)
                {
                    justAdded = true;
                }
            }
            if (justAdded)
                IsProcessingChanged?.Invoke(this);

            t.Start(new Container
            {
                Thread = t,
                Object = @object
            });
        }

        private void Run(object obj)
        {
            var cont = (Container)obj;
            try
            {
                Callback(cont.Object);
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                lock (LockHandle)
                {
                    Threads.Remove(cont.Thread);
                }
                Check();
            }
        }

        private void Check()
        {
            var el = default(T);
            var hasEl = false;
            var isEmpty = false;
            lock (LockHandle)
            {
                if (Objects.Count == 0)
                {
                    isEmpty = true;
                }
                else if (Threads.Count < DegreeOfParallelizm && Objects.Count > 0)
                {
                    el = Objects.Dequeue();
                    hasEl = true;
                }
            }
            if (hasEl)
                StartProcessing(el);
            if (isEmpty)
                IsProcessingChanged?.Invoke(this);
        }

        private class Container
        {
            public T Object { get; set; }
            public Thread Thread { get; set; }
        }
    }
}
