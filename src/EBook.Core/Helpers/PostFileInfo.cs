using EBook.Errors;
namespace EBook.Core.Helpers
{
    public class PostFileInfo : IMultipartParameter
    {
        public String ContentType { get; private set; }
        public String FileName { get; private set; }
        public byte[] Data { get; private set; }
        public String ParamName { get; private set; }
        public PostFileInfo(Stream data, String fileName, String paramName, String contentType)
        {
            Guard.ParamNotNull(data, "data");
            Guard.PropertyNotNullOrEmpty(fileName, "fileName");
            Guard.PropertyNotNullOrEmpty(paramName, "paramName");
            if (!(data.Position == 0 || data.CanSeek) || !data.CanRead)
                throw new InvalidArgumentException("Invalid stream. It must be readable, at the position 0 or seekable.");
            Data = data.Read();
            FileName = fileName;
            ParamName = paramName;
            if (String.IsNullOrWhiteSpace(contentType))
                DetectContentType();
            else
                ContentType = contentType;
        }
        public PostFileInfo(Stream data, String fileName, String paramName) : this(data, fileName, paramName, null) { }
        public PostFileInfo(byte[] data, String fileName, String paramName, String contentType)
        {
            Guard.ParamNotNull(data, "data");
            Guard.PropertyNotNullOrEmpty(fileName, "fileName");
            Guard.PropertyNotNullOrEmpty(paramName, "paramName");
            Data = data;
            FileName = fileName;
            ParamName = paramName;
            if (String.IsNullOrWhiteSpace(contentType))
                DetectContentType();
            else
                ContentType = contentType;
        }
        public PostFileInfo(byte[] data, String fileName, String paramName) : this(data, fileName, paramName, null) { }
        private void DetectContentType()
        {
            if (!String.IsNullOrWhiteSpace(FileName))
            {
                var helper = new FileSystemHelper();
                ContentType = helper.GetContentType(FileName);
            }
        }
        byte[] IMultipartParameter.Value
        {
            get { return Data; }
        }
    }
}