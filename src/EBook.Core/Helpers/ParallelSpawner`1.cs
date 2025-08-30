namespace EBook.Core.Helpers
{
    public class ParallelSpawner<T>
    {
        private Action<T> Callback;
        private int DegreeOfParralelizm;
        private object LockHandle = new object();
        private Queue<T> Objects;
        private int Processes;
        public event Action<ParallelSpawner<T>> IsProcessingChanged;
        public Boolean IsProcessing
        {
            get
            {
                lock (LockHandle)
                    return Processes > 0;
            }
        }
        public ParallelSpawner(Action<T> callback, Int32 degreeOfParralelizm)
        {
            Guard.ParamNotNull(callback, "callback");
            Guard.IsTrue<ArgumentOutOfRangeException>(degreeOfParralelizm >= 1, "degreeOfParralelizm");
            Callback = callback;
            DegreeOfParralelizm = degreeOfParralelizm;
            Objects = new Queue<T>();
        }
        public void Enqueue(T item)
        {
            lock (LockHandle)
            {
                if (Processes >= DegreeOfParralelizm)
                    Objects.Enqueue(item);
                else
                {
                    Processes++;
                    if (Processes == 1)
                        IsProcessingChanged.Try(x => x(this));
                    Callback.BeginInvoke(item, CallbackCompleted, null);
                }
            }
        }

        public void Stop()
        {
            lock (LockHandle)
            {
                Objects.Clear();
            }
        }

        private void CallbackCompleted(IAsyncResult result)
        {

            lock (LockHandle)
                if (Objects != null && Callback != null && Objects.Any() && Processes <= DegreeOfParralelizm)
                {
                    var item = Objects.Dequeue();
                    Callback.BeginInvoke(item, CallbackCompleted, null);
                }
                else
                {
                    Processes--;
                    if (Processes == 0)
                        IsProcessingChanged.Try(x => x(this));
                }
        }
    }
}
