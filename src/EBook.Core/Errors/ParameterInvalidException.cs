using System.Runtime.Serialization;

namespace EBook.Errors;

[Serializable]
public class ParameterInvalidException : BaseException
{
    public ParameterInvalidException() { }
    public ParameterInvalidException(String message) : base(message) { }
    public ParameterInvalidException(String message, Exception inner) : base(message, inner) { }
    protected ParameterInvalidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
