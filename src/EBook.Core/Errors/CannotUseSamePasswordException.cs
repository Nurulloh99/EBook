using System.Runtime.Serialization;
namespace   EBook.Errors;

[Serializable]
public class CannotUseSamePasswordException : BaseException
{
    public CannotUseSamePasswordException() { }
    public CannotUseSamePasswordException(String message) : base(message) { }
    public CannotUseSamePasswordException(String message, Exception inner) : base(message, inner) { }
    protected CannotUseSamePasswordException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}