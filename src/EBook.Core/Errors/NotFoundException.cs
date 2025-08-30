using System.Runtime.Serialization;

namespace EBook.Errors;

[Serializable]
public class NotFoundException : BaseException
{
    public NotFoundException() { }
    public NotFoundException(String message) : base(message) { }
    public NotFoundException(String message, Exception inner) : base(message, inner) { }
    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}