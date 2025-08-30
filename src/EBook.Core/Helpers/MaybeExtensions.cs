namespace EBook.Core.Helpers
{
    public static class MaybeExtensions
    {
        public static TResult With<T, TResult>(this T input, Func<T, TResult> expr)
        {
            if (input == null)
                return default;
            return expr(input);
        }
        public static TResult Return<T, TResult>(this T input, Func<T, TResult> expr, TResult fallBackValue)
        {
            if (input == null)
                return fallBackValue;
            var val = expr(input);
            if (val == null)
                return fallBackValue;
            return val;
        }
        public static T Try<T>(this T input, Action<T> action)
        {
            if (input != null)
                action(input);
            return input;
        }
        public static void TryAs<T>(this Object obj, Action<T> action) where T : class
        {
            Guard.ParamNotNull(action, "action");
            var casted = obj as T;
            if (casted != null)
                action(casted);
        }

        public static T Or<T>(this T target, T fallbackValue)
        {
            if (target == null)
                return fallbackValue;
            return target;
        }

        public static String OrFollowingIfNull(this String value, String alt, params String[] alts)
        {
            if (!String.IsNullOrWhiteSpace(value))
                return value;
            if (!String.IsNullOrWhiteSpace(alt))
                return alt;
            return alts.FirstOrDefault(x => !String.IsNullOrWhiteSpace(x));
        }
        public static Exception TrySafe<T>(this T obj, Action<T> action)
        {
            if (obj != null)
                try
                {
                    action(obj);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            return null;
        }
    }
}