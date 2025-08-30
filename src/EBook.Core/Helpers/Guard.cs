using EBook.Errors;
namespace EBook.Core.Helpers
{
    public static class Guard
    {
        /// <summary>
        /// This method is used to guard application controlled 
        /// methods and constructors. This guard failure will mean 
        /// the application issue
        /// </summary>
        /// <param name="obj">Object to validate</param>
        /// <param name="argName">Name of the parameter</param>
        public static void ArgNotNull(object obj, string argName)
        {
            if (obj == null)
                throw new ArgumentNullException(argName);
        }

        public static void ParamNotNull(object obj, string argName)
        {
            if (obj == null)
                throw new ParameterInvalidException(argName);
        }

        public static void PropertyNotNull(object obj, string propertyPath)
        {
            if (obj == null)
                throw new ParameterInvalidException(string.Format("Property path {0} cannot be null", propertyPath));
        }

        public static void ArgNotNullOrEmpty(string value, string argName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(argName);
        }

        public static void PropertyNotNullOrEmpty(string value, string propertyPath)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ParameterInvalidException(string.Format("Property path {0} cannot be null or empty", propertyPath));
        }

        public static void IsTrue<T>(bool value) where T : Exception, new()
        {
            if (!value)
                throw new T();
        }
        public static void IsTrue<T>(bool value, T error) where T : Exception
        {
            if (!value)
                throw error;
        }

        public static void IsTrue<T>(bool value, string message) where T : Exception
        {
            if (!value)
            {
                throw ErrorHelper<T>.Instance.Create(message);
            }
        }
    }
}