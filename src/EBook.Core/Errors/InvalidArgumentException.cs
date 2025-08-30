using System.Runtime.Serialization;
namespace EBook.Errors;

[Serializable]
public class InvalidArgumentException : BaseException
{
    public InvalidArgumentException() { }
    public InvalidArgumentException(String message) : base(message) { }
    public InvalidArgumentException(String message, Exception ex) : base(message, ex) { }
    protected InvalidArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}