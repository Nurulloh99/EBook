using EBook.Errors;
namespace EBook.Core.Helpers
{
    public class FsFile
    {
        protected String FullFileName;
        public virtual String Text
        {
            get { return File.ReadAllText(FullFileName); }
        }
        public virtual Stream InMemoryStream
        {
            get { return new MemoryStream(File.ReadAllBytes(FullFileName)); }
        }
        public virtual byte[] Bytes
        {
            get { return File.ReadAllBytes(FullFileName); }
        }
        public virtual String FullFilePath
        {
            get { return FullFileName; }
        }
        public FsFile(String fileName)
        {
            Guard.ParamNotNull(fileName, "fileName");
            FullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(FullFileName))
                throw new NotFoundException(String.Format("File \"{0}\" not found", FullFileName));
        }
        protected FsFile() { }
    }
}