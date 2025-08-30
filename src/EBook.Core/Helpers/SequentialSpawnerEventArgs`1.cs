namespace EBook.Core.Helpers
{
    public class SequentialSpawnerEventArgs<T> : EventArgs
    {
        public int Count { get; private set; }
        public T Object { get; private set; }
        public SequentialSpawnerEventArgs(int count, T @object)
        {
            Count = count;
            Object = @object;
        }
    }
}