using System.Linq.Expressions;

namespace EBook.Core.Helpers
{
    internal class ErrorHelper<T> where T : Exception
    {
        private static readonly Func<string, Exception, T> throwerWithInner;
        private static readonly Func<string, T> throwerWithMessage;
        private static readonly Func<T> thrower;

        private static readonly Lazy<ErrorHelper<T>> instanceLazy = new Lazy<ErrorHelper<T>>();
        public static ErrorHelper<T> Instance => instanceLazy.Value;

        static ErrorHelper()
        {
            var strType = typeof(string);
            var excType = typeof(Exception);
            var type = typeof(T);
            var emptyConstructor = type.GetConstructor(new Type[0]);

            var body = Expression.New(emptyConstructor);
            var throwerExpr = Expression.Lambda<Func<T>>(body);
            thrower = throwerExpr.Compile();

            var strConstructor = type.GetConstructor(new[] { strType });
            var strParam = Expression.Parameter(strType, "x");
            var strBody = Expression.New(strConstructor, strParam);
            var strThrowerExpr = Expression.Lambda<Func<string, T>>(strBody, strParam);
            throwerWithMessage = strThrowerExpr.Compile();

            var strExConstructor = type.GetConstructor(new[] { strType, excType });
            var excParam = Expression.Parameter(excType, "ex");
            var strExBody = Expression.New(strExConstructor, strParam, excParam);
            var strExExpr = Expression.Lambda<Func<string, Exception, T>>(strExBody, strParam, excParam);
            throwerWithInner = strExExpr.Compile();
        }

        public T Create(string message, Exception ex)
        {
            return throwerWithInner(message, ex);
        }

        public T Create(string message)
        {
            return throwerWithMessage(message);
        }

        public T Create()
        {
            return thrower();
        }
    }
}
