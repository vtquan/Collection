using System;
using System.IO;
using System.IO.Compression;

namespace MyBookLibrary.Helpers
{
    public class FileHelper
    {
        public static string GetFileContent(bool isText, ZipArchiveEntry extendedZipEntry)
        {
            using (var stream = new MemoryStream())
            {
                extendedZipEntry.Open().CopyTo(stream);

                stream.Position = 0;

                if (isText)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }

                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}