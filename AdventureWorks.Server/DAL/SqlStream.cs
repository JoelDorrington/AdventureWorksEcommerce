using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.SqlClient;

namespace AdventureWorks.Server.DAL
{
    public class SqlReaderStream : Stream
    {
        private readonly ISqlClientFactory _sqlClientFactory;
        private SqlConnection connection;
        private SqlDataReader? reader;
        private SqlCommand command;
        private const int fileNameIndex = 0;
        private const int dataIndex = 1;
        private long position;
        private bool disposed = false;
        public string FileName { get; set; } = "";
        public string Mimetype { get; set; } = "application/octet-stream";

        public async Task ExecuteReaderAsync()
        {
            await connection.OpenAsync();
            reader = await command.ExecuteReaderAsync(System.Data.CommandBehavior.SequentialAccess);
            if(!await reader.ReadAsync()) throw new InvalidOperationException("No data found.");
            if (fileNameIndex >= 0)
            {
                FileName = reader.GetString(fileNameIndex);
                var mapper = new FileExtensionContentTypeProvider();
                if(mapper.TryGetContentType(FileName, out string? mimeType))
                {
                    Mimetype = mimeType;
                }
            }
        }

        public SqlReaderStream(ISqlClientFactory sqlClientFactory, SqlCommand command)
        {
            this._sqlClientFactory = sqlClientFactory;
            this.command = command;
            connection = _sqlClientFactory.GetConnection();
            command.Connection = connection;
        }

        public override long Position
        {
            get { return position; }
            set { throw new NotImplementedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if(reader == null) throw new InvalidOperationException("Reader is not initialized.");
            long bytesRead = reader.GetBytes(dataIndex, position, buffer, offset, count);
            position += bytesRead;
            return (int)bytesRead;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                if(reader != null) reader.Dispose();
                command.Dispose();
                connection.Dispose();
            }
            base.Dispose(disposing);
            disposed = true;
        }
    }
}
