using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Songer.WebAPI.Helpers
{
    public class HttpFileStream
    {
        public const int ReadStreamBufferSize = 9 * 1024 * 1024;

        private readonly string _path;

        public HttpFileStream(string path)
        {
            _path = path;
        }

        public async void WriteToStream(Stream outputStream, ActionContext context)
        {
            try
            {
                var buffer = new byte[ReadStreamBufferSize];

                using (Stream stream = new FileStream(_path, FileMode.Open, FileAccess.Read))
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, ReadStreamBufferSize);
                        await outputStream.WriteAsync(buffer, 0, count);
                    }
                    while (stream.CanRead && count > 0);
                }
            }
            catch
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}
