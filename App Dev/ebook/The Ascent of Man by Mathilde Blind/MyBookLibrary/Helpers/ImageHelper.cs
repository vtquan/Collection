using System.IO;
using System.Linq;

namespace MyBookLibrary.Helpers
{
    public class ImageHelper
    {
        public static string GetExtensionForMimeType(string mimeType)
        {
            switch (mimeType)
            {
                case "image/jpeg":
                    return ".jpeg";

                case "image/png":
                    return ".png";

                case "image/gif":
                    return ".gif";
            }

            throw new InvalidDataException("Provided mimeType is not supported.");
        }

        public static string GetMimeTypeForImage(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            switch (extension)
            {
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";

                case ".png":
                    return "image/png";

                case ".gif":
                    return "image/gif";
            }

            throw new InvalidDataException("Provided file is not supported.");
        }

        public static bool IsFileOfTypeImage(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            var imgExtensions = new[] {".jpeg", ".jpg", ".png", ".gif"};

            return imgExtensions.Contains(extension);
        }
    }
}