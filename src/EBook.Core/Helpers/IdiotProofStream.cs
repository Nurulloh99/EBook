namespace EBook.Core.Helpers
{
    /// <summary>
    ///     This class is used in cases with methods when the streams
    ///     for some strange reasons are disposed.
    ///     This stream is resistant to such kind of disposing and will contain
    ///     the data even after calling Dispose method.
    /// </summary>
    public class IdiotProofStream : Stream
    {
        private readonly Stream InternalStream;

        /// <summary>
        /// Creates the instance with <see cref="MemoryStream"/> 
        /// as an underlying stream
        /// </summary>
        public IdiotProofStream()
        {
            InternalStream = new MemoryStream();
        }

        /// <summary>
        /// Creates the instance with <see cref="MemoryStream"/> 
        /// as an underlying stream filled in with the buffer
        /// </summary>
        /// <param name="buffer"></param>
        public IdiotProofStream(byte[] buffer)
        {
            InternalStream = new MemoryStream(buffer);
        }

        /// <summary>
        /// Creates the instance with the provided underlying stream
        /// </summary>
        /// <param name="input"></param>
        public IdiotProofStream(Stream input)
        {
            Guard.ParamNotNull(input, nameof(input));
            InternalStream = input;
        }

        public override bool CanRead => InternalStream.CanRead;

        public override bool CanSeek => InternalStream.CanSeek;

        public override bool CanWrite => InternalStream.CanWrite;

        public override void Flush() =>
            InternalStream.Flush();

        public override long Length => InternalStream.Length;

        public override long Position
        {
            get => InternalStream.Position;
            set => InternalStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count) =>
            InternalStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) =>
            InternalStream.Seek(offset, origin);

        public override void SetLength(long value) =>
            InternalStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) =>
            InternalStream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing) { }

        public override void Close() { }

        public Stream GetUnderlyingStream() => InternalStream;
    }
}