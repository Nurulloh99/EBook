namespace EBook.Core.Helpers
{
    public class AccumulatingSpawner<T> : IDisposable
    {
        private Queue<T> Container;
        private Timer Timer;
        private TimeSpan _SlidingWaitDuration;
        private TimeSpan TimerInterval;
        private Object Locker = new Object();
        private DateTime? LastArrival;
        public TimeSpan SlidingWaitDuration
        {
            get { return _SlidingWaitDuration; }
        }
        public event Action<T[]> Spawn;

        public AccumulatingSpawner(TimeSpan slidingWaitDuration, TimeSpan timerInterval)
        {
            if (slidingWaitDuration <= TimeSpan.Zero)
                throw new Exception("sliding wait duration cannot be less or equal to 0");
            if (timerInterval <= TimeSpan.Zero)
                throw new Exception("timerInterval cannot be less or equal to 0");
            _SlidingWaitDuration = slidingWaitDuration;
            TimerInterval = timerInterval;
            Container = new Queue<T>();
            Timer = new Timer(TimerElapsed, null, TimeSpan.Zero, timerInterval);
        }
        public void TimerElapsed(Object state)
        {
            var data = (List<T>)null;
            lock (Locker)
            {
                var now = DateTime.UtcNow;
                if (Container.Count > 0 && LastArrival.HasValue && LastArrival.Value.Add(_SlidingWaitDuration) < now)
                {
                    LastArrival = null;
                    data = Container.ToList();
                    Container.Clear();
                }
            }
            var handle = Spawn;
            if (handle != null && data != null)
                handle(data.ToArray());
        }
        public void Add(T @object)
        {
            lock (Locker)
            {
                LastArrival = DateTime.UtcNow;
                Container.Enqueue(@object);
            }
        }
        public void Dispose()
        {
            lock (Locker)
            {
                if (Timer != null)
                {
                    Timer.Change(Timeout.Infinite, Timeout.Infinite);
                    Timer.Dispose();
                }
                if (Container != null)
                    Container.Clear();
            }
        }
    }
}