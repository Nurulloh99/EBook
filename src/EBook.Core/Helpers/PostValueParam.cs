using System.Text;

namespace EBook.Core.Helpers
{
    public class PostValueParam : IMultipartParameter
    {
        public String ParamName { get; private set; }
        public String Value { get; private set; }
        public PostValueParam(String paramName, String value)
        {
            Guard.PropertyNotNullOrEmpty(paramName, "paramName");
            Guard.PropertyNotNullOrEmpty(value, "value");
            ParamName = paramName;
            Value = value;
        }
        byte[] IMultipartParameter.Value
        {
            get { return Value.Return(x => Encoding.UTF8.GetBytes(Value), null); }
        }
        String IMultipartParameter.FileName
        {
            get { return null; }
        }
        String IMultipartParameter.ContentType
        {
            get { return null; }
        }
    }
}