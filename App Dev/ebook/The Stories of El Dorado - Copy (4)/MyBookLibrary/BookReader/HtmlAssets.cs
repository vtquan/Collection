using System.IO;
using System.Reflection;

namespace MyBookLibrary.BookReader
{
    public class HtmlAssets
    {
        private static string _htmlTemplate;

        public static string HtmlTemplate
        {
            get
            {
                if (_htmlTemplate == null)
                {
                    Assembly assembly = typeof (HtmlAssets).GetTypeInfo().Assembly;

                    using (
                        Stream stream = assembly.GetManifestResourceStream("MyBookLibrary.Assets.ePubPageTemplate.html")
                        )
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            _htmlTemplate = reader.ReadToEnd();
                        }
                    }
                }

                return _htmlTemplate;
            }
        }
    }
}