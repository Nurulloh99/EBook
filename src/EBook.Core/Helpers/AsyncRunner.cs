namespace EBook.Core.Helpers
{
    public class AsyncRunner : IDisposable
    {
        protected readonly Action Action;
        protected AutoResetEvent Signaller;

        public AsyncRunner(Action action, bool wait)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            Action = action;

            if (wait)
                Signaller = new AutoResetEvent(false);
        }

        protected AsyncRunner() { }

        public void Dispose()
        {
            Signaller.SafeDispose();
            Signaller = null;
        }

        public void Run()
        {
            var t = new Thread(AsyncRun);
            t.Start(this);
            Signaller?.WaitOne();
        }

        protected virtual void AsyncRun(object obj)
        {
            var runner = obj as AsyncRunner;
            runner.Action();
            runner.Signaller?.Set();
        }
    }
}