using System.Runtime.Serialization;

namespace EBook.Errors;

[Serializable]
public class NoConnectionException : BaseException
{
    public NoConnectionException() { }
    public NoConnectionException(String message) : base(message) { }
    public NoConnectionException(String message, Exception inner) : base(message, inner) { }
    protected NoConnectionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}