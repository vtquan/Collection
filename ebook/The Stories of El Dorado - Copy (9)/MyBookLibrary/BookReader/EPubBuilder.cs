using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyBookLibrary.Helpers;
using Windows.Security.Cryptography;
using Windows.Storage;

namespace MyBookLibrary.BookReader
{
    public class EPubBuilder
    {
        private string _contentPath;

        private string _tocFilePath;

        private void GetCoverImage(EPub epub, string type, string fileName, ZipArchive epubFile)
        {
            if (type == "cover" && !ImageHelper.IsFileOfTypeImage(fileName)) type = "page";

            if (type != "cover")
            {
                var fileextentions = new[] {".htm", ".html", ".xhtml"};

                if (fileextentions.Contains(Path.GetExtension(fileName)))
                {
                    string content;

                    if (epub.Content.ContainsKey(fileName))
                    {
                        content = epub.Content[fileName].Content;
                        epub.Content.Remove(fileName);
                    }
                    else
                    {
                        ReadOnlyCollection<ZipArchiveEntry> entries = epubFile.Entries;
                        ZipArchiveEntry zipArchiveEntry =
                            entries.FirstOrDefault(
                                e =>
                                e.FullName.Equals(string.Concat(_contentPath, fileName),
                                                  StringComparison.OrdinalIgnoreCase));

                        if (zipArchiveEntry == null) return;

                        content = FileHelper.GetFileContent(true, zipArchiveEntry);
                    }

                    MatchCollection matchCollections = Regex.Matches(content,
                                                                     "(?<prefix>(xlink:href|src)\\s*=\\s*\")(?<up_nav>(\\.+/)*)(?<link>[^\"]*)\"",
                                                                     RegexHelper.RegexOpts);

                    if (matchCollections.Count == 1)
                    {
                        string data = WebUtility.UrlDecode(matchCollections[0].Groups["link"].ToString());
                        if (data.Contains("data"))
                        {
                            string item = data.Substring(0, data.IndexOf(','));
                            string mimeTypeData = item.Substring(item.IndexOf(':') + 1,
                                                                 item.IndexOf(';') - item.IndexOf(':') - 1);
                            string contentData = data.Substring(item.Length + 1, data.Length - item.Length - 1);
                            string fileNameData = string.Concat(".", mimeTypeData.Remove(0, 6));

                            var ePubEntry = new EPubEntry
                                {
                                    FileName = string.Concat("generated_cover", fileNameData),
                                    MimeType = mimeTypeData,
                                    Content = contentData
                                };

                            epub.CoverImage = ePubEntry;
                            epub.ExtraData.Add(string.Concat("generated_cover", fileName), epub.CoverImage);

                            return;
                        }

                        if (!epub.ExtraData.ContainsKey(data))
                        {
                            ReadOnlyCollection<ZipArchiveEntry> zipArchiveEntries = epubFile.Entries;
                            ZipArchiveEntry zipArchiveEntry =
                                zipArchiveEntries.FirstOrDefault(
                                    e =>
                                    e.FullName.Equals(string.Concat(_contentPath, data),
                                                      StringComparison.OrdinalIgnoreCase));

                            if (zipArchiveEntry == null) return;

                            string fileContent = FileHelper.GetFileContent(false, zipArchiveEntry);

                            var mimeTypeForImage = new EPubEntry
                                {
                                    FileName = data,
                                    MimeType = ImageHelper.GetMimeTypeForImage(data),
                                    Content = fileContent
                                };

                            epub.CoverImage = mimeTypeForImage;
                            epub.ExtraData.Add(data, epub.CoverImage);

                            return;
                        }

                        epub.CoverImage = epub.ExtraData[data];
                    }
                }
            }
            else
            {
                if (epub.ExtraData.ContainsKey(fileName))
                {
                    epub.CoverImage = epub.ExtraData[fileName];

                    return;
                }

                if (ImageHelper.IsFileOfTypeImage(fileName))
                {
                    ReadOnlyCollection<ZipArchiveEntry> entries = epubFile.Entries;
                    ZipArchiveEntry zipArchiveEntry =
                        entries.FirstOrDefault(
                            e =>
                            e.FullName.Equals(string.Concat(_contentPath, fileName), StringComparison.OrdinalIgnoreCase));

                    if (zipArchiveEntry == null) return;

                    string fileContent = FileHelper.GetFileContent(false, zipArchiveEntry);

                    var ePubEntry = new EPubEntry
                        {
                            FileName = fileName,
                            MimeType = ImageHelper.GetMimeTypeForImage(fileName),
                            Content = fileContent
                        };

                    epub.CoverImage = ePubEntry;
                }
            }
        }

        public async Task<EPub> GetEPub(StorageFile ePubFile)
        {
            var epub = new EPub(ePubFile.DisplayName);

            using (var memoryStream = new MemoryStream())
            {
                using (Stream stream = await ePubFile.OpenStreamForReadAsync())
                {
                    await stream.CopyToAsync(memoryStream);
                }

                var epubFile = new ZipArchive(memoryStream);
                string opfFilePath = GetOpfFilePath(epubFile);

                if (string.IsNullOrEmpty(opfFilePath)) throw new Exception("Invalid EPub file.");

                Match m = Regex.Match(opfFilePath, "^.*/", RegexHelper.RegexOpts);
                _contentPath = m.Success ? m.Value : "";

                LoadMetaData(epub, epubFile, opfFilePath);

                if (_tocFilePath != null) LoadTableOfContents(epub);
            }
            return epub;
        }

        private List<NavigationPoint> GetNavigationChildren(EPub epub, IEnumerable<XElement> elements,
                                                            XNamespace nameSpace)
        {
            XElement[] xElements = elements as XElement[] ?? elements.ToArray();

            var navPoints = new List<NavigationPoint>(xElements.Count());

            if (!xElements.Any()) return navPoints;

            navPoints.AddRange(xElements.Select(navPoint =>
                {
                    var value = new NavigationPoint
                        {
                            Id = navPoint.Attribute("id").Value,
                            Title = navPoint.Element(nameSpace + "navLabel").Element(nameSpace + "text").Value,
                            Source =
                                WebUtility.UrlDecode(navPoint.Element(nameSpace + "content").Attribute("src").Value),
                            Order = int.Parse(navPoint.Attribute("playOrder").Value)
                        };

                    return value;
                }));

            navPoints.AddRange(
                navPoints.FindAll(
                    navPoint =>
                    epub.Content.Values.FirstOrDefault(content => content.FileName == navPoint.Source) != null));

            return navPoints;
        }

        private string GetOpfFilePath(ZipArchive epubFile)
        {
            ZipArchiveEntry entry =
                (epubFile.Entries).FirstOrDefault(
                    e => e.FullName.Equals("meta-inf/container.xml", StringComparison.OrdinalIgnoreCase));

            if (entry != null)
            {
                string data;
                XElement element;

                using (var stream = new MemoryStream())
                {
                    entry.Open().CopyTo(stream);

                    stream.Position = 0;
                    element = XElement.Load(stream);

                    stream.Position = 0;
                    data = new StreamReader(stream).ReadToEnd();
                }

                XNamespace mynamespace = (element.Attribute("xmlns") != null)
                                             ? (element.Attribute("xmlns").Value)
                                             : XNamespace.None;

                if (mynamespace != XNamespace.None)
                {
                    return element.Descendants((mynamespace + "rootfile"))
                                  .FirstOrDefault(
                                      p => (p.Attribute("media-type") != null) &&
                                           p.Attribute("media-type")
                                            .Value.Equals("application/oebps-package+xml",
                                                          StringComparison.OrdinalIgnoreCase))
                                  .Attribute("full-path")
                                  .Value;
                }

                XDocument document = XDocument.Parse(data);
                document.Root.Add(new XAttribute("xmlns", "urn:oasis:names:tc:opendocument:xmlns:container"));
                document.Root.Attributes((XNamespace.Xmlns + "odfc")).Remove();
                element = XElement.Parse(document.ToString());
                mynamespace = (element.Attribute("xmlns") != null)
                                  ? (element.Attribute("xmlns").Value)
                                  : XNamespace.None;

                if (mynamespace != XNamespace.None)
                {
                    return element.Descendants((mynamespace + "rootfile"))
                                  .FirstOrDefault(
                                      p => (p.Attribute("media-type") != null) &&
                                           p.Attribute("media-type")
                                            .Value.Equals("application/oebps-package+xml",
                                                          StringComparison.OrdinalIgnoreCase))
                                  .Attribute("full-path")
                                  .Value;
                }
            }

            return null;
        }

        private void LoadMetaData(EPub epub, ZipArchive epubFile, string opfFilePath)
        {
            XElement element;
            ZipArchiveEntry entry = (epubFile.Entries).FirstOrDefault(
                e => e.FullName.Equals(opfFilePath, StringComparison.OrdinalIgnoreCase));

            if (entry == null) throw new Exception("Invalid epub file.");

            using (var stream = new MemoryStream())
            {
                entry.Open().CopyTo(stream);
                stream.Position = 0;
                element = XElement.Load(stream);
            }

            XNamespace xNamespace = (element.Attribute("xmlns") != null)
                                        ? (element.Attribute("xmlns").Value)
                                        : XNamespace.None;
            string uniqueIdentifier = element.Attribute("unique-identifier").Value;

            epub.Uuid = element.Elements((xNamespace + "metadata"))
                               .Elements()
                               .FirstOrDefault(
                                   e => ((e.Name.LocalName == "identifier") && (e.Attribute("id") != null)) &&
                                        (e.Attribute("id").Value == uniqueIdentifier))
                               .Value;

            if (epub.Uuid.Contains("uuid:"))
            {
                epub.Uuid = epub.Uuid.Remove(0, epub.Uuid.IndexOf("uuid:", StringComparison.Ordinal) + 5);
            }

            if (!string.Equals(WebUtility.UrlEncode(epub.Uuid), epub.Uuid, StringComparison.OrdinalIgnoreCase))
            {
                epub.Uuid =
                    CryptographicBuffer.EncodeToHexString(CryptographicBuffer.ConvertStringToBinary(epub.Uuid, 0));
            }

            foreach (XElement elementItem in from e in element.Elements((xNamespace + "metadata")).Elements() select e)
            {
                switch (elementItem.Name.LocalName)
                {
                    case "title":
                        epub.Title.Add(elementItem.Value);
                        break;

                    case "creator":
                        epub.Creator.Add(elementItem.Value);
                        break;

                    case "date":

                        XAttribute attribute =
                            elementItem.Attributes()
                                       .FirstOrDefault(a => a.Name.LocalName == "event");
                        if (attribute != null)
                        {
                            epub.Date.Add(new DateData(attribute.Value, elementItem.Value));
                        }
                        break;

                    case "publisher":
                        epub.Publisher.Add(elementItem.Value);
                        break;

                    case "subject":
                        epub.Subject.Add(elementItem.Value);
                        break;

                    case "source":
                        epub.Source.Add(elementItem.Value);
                        break;

                    case "rights":
                        epub.Rights.Add(elementItem.Value);
                        break;

                    case "description":
                        epub.Description.Add(elementItem.Value);
                        break;

                    case "contributor":
                        epub.Contributer.Add(elementItem.Value);
                        break;

                    case "type":
                        epub.Type.Add(elementItem.Value);
                        break;

                    case "format":
                        epub.Format.Add(elementItem.Value);
                        break;

                    case "identifier":
                        epub.Id.Add(elementItem.Value);
                        break;

                    case "language":
                        epub.Language.Add(elementItem.Value);
                        break;

                    case "relation":
                        epub.Relation.Add(elementItem.Value);
                        break;

                    case "coverage":
                        epub.Coverage.Add(elementItem.Value);
                        break;
                }
            }

            LoadOpfManifest(epub, epubFile, element, xNamespace);
        }

        private void LoadOpfManifest(EPub epub, ZipArchive epubFile, XElement contentOpf, XNamespace xNamespace)
        {
            var items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (XElement xElement in contentOpf.Elements(xNamespace + "spine").Elements())
            {
                IEnumerable<XElement> xElements = contentOpf.Elements(xNamespace + "manifest").Elements();
                string key =
                    WebUtility.UrlDecode(
                        xElements.FirstOrDefault(
                            e => e.Attribute("id").Value == xElement.Attribute("idref").Value)
                                 .Attribute("href")
                                 .Value);

                ZipArchiveEntry zipArchiveEntry =
                    epubFile.Entries.FirstOrDefault(
                        e =>
                        e.FullName.Equals(string.Concat(_contentPath, key), StringComparison.OrdinalIgnoreCase));

                if (zipArchiveEntry == null) throw new Exception("Invalid EPub file.");

                if (!epub.Content.ContainsKey(key))
                {
                    var ePubEntry = new EPubEntry
                        {
                            FileName = key,
                            Content = FileHelper.GetFileContent(true, zipArchiveEntry)
                        };
                    epub.Content.Add(key, ePubEntry);
                }

                if (items.Contains(xElement.Attribute("idref").Value)) continue;

                items.Add(xElement.Attribute("idref").Value);
            }

            IEnumerable<XElement> xElements1 =
                contentOpf.Elements(xNamespace + "manifest")
                          .Elements()
                          .Where(e => !items.Contains(e.Attribute("id").Value));

            foreach (XElement xElement in xElements1)
            {
                string href = WebUtility.UrlDecode(xElement.Attribute("href").Value);
                ZipArchiveEntry zipArchiveEntry =
                    epubFile.Entries.FirstOrDefault(
                        e =>
                        e.FullName.Equals(string.Concat(_contentPath, href), StringComparison.OrdinalIgnoreCase));
                if (zipArchiveEntry == null)
                {
                    continue;
                }

                string value = xElement.Attribute("media-type").Value;

                bool flag = (!value.ToLowerInvariant().Contains("image") || value.ToLowerInvariant().Contains("svg+xml"));

                if (!epub.ExtraData.ContainsKey(href))
                {
                    var fileContent = new EPubEntry
                        {
                            FileName = href,
                            MimeType = value,
                            Content = FileHelper.GetFileContent(flag, zipArchiveEntry)
                        };

                    epub.ExtraData.Add(href, fileContent);
                }

                if (
                    !string.Equals(xElement.Attribute("media-type").Value, "application/x-dtbncx+xml",
                                   StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                _tocFilePath = WebUtility.UrlDecode(xElement.Attribute("href").Value);
            }

            IEnumerable<XElement> xElements2 = contentOpf.Elements(xNamespace + "guide").Elements();
            foreach (XElement xElement in xElements2)
            {
                string href = WebUtility.UrlDecode(xElement.Attribute("href").Value);
                string lower = xElement.Attribute("type").Value.ToLower();
                if (lower != "cover" && lower != "title-page")
                {
                    continue;
                }
                GetCoverImage(epub, lower, href, epubFile);
            }
        }

        private void LoadTableOfContents(EPub epub)
        {
            EPubEntry item = epub.ExtraData[_tocFilePath];

            if (item == null) return;

            XElement xElement = XElement.Parse(item.Content);
            XNamespace xNamespace = xElement.Attribute("xmlns") != null
                                        ? xElement.Attribute("xmlns").Value
                                        : XNamespace.None;

            if (xNamespace != XNamespace.None)
            {
                epub.TableOfContents = GetNavigationChildren(epub,
                                                             xElement.Element(xNamespace + "navMap")
                                                                     .Elements(xNamespace + "navPoint"), xNamespace);
                return;
            }

            XDocument xDocument = XDocument.Parse(item.Content);
            xDocument.Root.Attributes(XNamespace.Xmlns + "ncx").Remove();
            xElement = XElement.Parse(xDocument.ToString());
            xNamespace = xElement.Attribute("xmlns") != null ? xElement.Attribute("xmlns").Value : XNamespace.None;

            if (xNamespace != XNamespace.None)
            {
                epub.TableOfContents = GetNavigationChildren(epub,
                                                             xElement.Element(xNamespace + "navMap")
                                                                     .Elements(xNamespace + "navPoint"), xNamespace);
            }
        }
    }
}