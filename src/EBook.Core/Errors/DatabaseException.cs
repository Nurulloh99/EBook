using System.Runtime.Serialization;

namespace EBook.Errors;

[Serializable]
public class DatabaseException : BaseException
{
    public DatabaseException() { }
    public DatabaseException(String message) : base(message) { }
    public DatabaseException(String message, Exception inner) : base(message, inner) { }
    protected DatabaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
