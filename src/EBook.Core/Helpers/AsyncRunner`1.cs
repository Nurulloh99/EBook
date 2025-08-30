namespace EBook.Core.Helpers
{
    public class AsyncRunner<T> : AsyncRunner
    {
        public T RunResult { get; private set; }
        public Func<T> Function { get; private set; }

        public AsyncRunner(Func<T> function, bool wait)
        {
            if (function is null)
            {
                throw new ArgumentNullException(nameof(function));
            }
            Function = function;
            if (wait)
                Signaller = new AutoResetEvent(false);
        }

        protected override void AsyncRun(object obj)
        {
            var runner = obj as AsyncRunner<T>;
            if (runner.Action != null)
                runner.Action();
            else if (runner.Function != null)
                RunResult = runner.Function();
            runner.Signaller?.Set();
        }
    }
}
