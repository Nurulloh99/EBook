namespace EBook.Core.Helpers
{
    public class ErrorHelper
    {
        public TException Create<TException>()
            where TException : Exception
        {
            return ErrorHelper<TException>.Instance.Create();
        }

        public TException Create<TException>(string message)
            where TException : Exception
        {
            return ErrorHelper<TException>.Instance.Create(message);
        }

        public TException Create<TException>(string message, Exception inner)
            where TException : Exception
        {
            return ErrorHelper<TException>.Instance.Create(message, inner);
        }

        public static Exception Create(Type exceptionType, string message, Exception inner)
        {
            return (Exception)Activator.CreateInstance(exceptionType, message, inner);
        }
    }
}
