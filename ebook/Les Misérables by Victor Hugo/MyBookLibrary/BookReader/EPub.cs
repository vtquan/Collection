using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyBookLibrary.Collections;
using MyBookLibrary.Helpers;

namespace MyBookLibrary.BookReader
{
    public class EPub
    {
        public EPub(string ePubName)
        {
            EPubName = ePubName;
            Id = new List<string>();
            Title = new List<string>();
            Language = new List<string>();
            Creator = new List<string>();
            Description = new List<string>();
            Date = new List<DateData>();
            Publisher = new List<string>();
            Contributer = new List<string>();
            Type = new List<string>();
            Format = new List<string>();
            Subject = new List<string>();
            Source = new List<string>();
            Relation = new List<string>();
            Coverage = new List<string>();
            Rights = new List<string>();
            Content = new OrderedDictionary<string, EPubEntry>();
            ExtraData = new OrderedDictionary<string, EPubEntry>();
            TableOfContents = new List<NavigationPoint>();
        }

        internal OrderedDictionary<string, EPubEntry> Content { get; set; }

        public List<string> Contributer { get; set; }

        public List<string> Coverage { get; set; }

        public EPubEntry CoverImage { get; set; }

        public List<string> Creator { get; set; }

        public List<DateData> Date { get; set; }

        public List<string> Description { get; set; }

        public string EPubName { get; set; }

        internal OrderedDictionary<string, EPubEntry> ExtraData { get; set; }

        public List<string> Format { get; set; }

        public List<string> Id { get; set; }

        public List<string> Language { get; set; }

        public List<string> Publisher { get; set; }

        public List<string> Relation { get; set; }

        public List<string> Rights { get; set; }

        public List<string> Source { get; set; }

        public List<string> Subject { get; set; }

        public List<NavigationPoint> TableOfContents { get; set; }

        public List<string> Title { get; set; }

        public List<string> Type { get; set; }

        public string Uuid { get; set; }

        public string GetHtml(string htmlTemplate)
        {
            var stringBuilder = new StringBuilder();

            for (int num = 0; num < Content.Count; num++)
            {
                stringBuilder.Append(string.Format("<div class=\"epub_section\" InTOC=\"{0}\">{1}</div>",
                                                   TableOfContents.FirstOrDefault(x => x.Source == Content[num].FileName) !=
                                                   null, ""));
            }

            return htmlTemplate.Replace("[CONTENT]", stringBuilder.ToString());
        }

        public async Task<string> GetHtmlForSection(int section)
        {
            string result = "";

            Match match = Regex.Match(Content[section].Content, "<body[^>]*>(?<body>.+)</body>",
                                      RegexHelper.RegexOptsSingleline);

            if (match.Success)
            {
                result = await Task.Run(() => ProcessLinks(match.Groups["body"].Value, section));
            }

            return result;
        }

        public string GetTitle()
        {
            if (Title.Count <= 0) return EPubName;

            if (Creator.Count <= 0) return Title[0];

            return string.Concat(Creator[0], " - ", Title[0]);
        }

        private string ProcessLinks(string text, int processedSection)
        {
            if (text == null) return null;

            Evaluate evaluate = (match, contentData) =>
                {
                    string str = string.Concat(match.Groups["Anchor"].Value, "javascript:void(0);\" onclick=\"");

                    if (!match.Groups["Protocol"].Success)
                    {
                        int num;

                        if (!match.Groups["TOC"].Success)
                        {
                            num = contentData.IndexOfKey(match.Groups["FullLink"].Value);
                            if (num == -1)
                            {
                                return string.Concat(match.Groups["Anchor"].Value, "javascript:void(0);");
                            }

                            str = string.Concat(str, "handleInternalRef(", num.ToString());
                        }
                        else
                        {
                            str = string.Concat(str, "handleInternalTOCRef('", match.Groups["TOC"].Value, "'");
                            string value = match.Groups["Link"].Value;
                            num = (!string.IsNullOrEmpty(value)
                                       ? contentData.IndexOfKey(match.Groups["Link"].Value)
                                       : processedSection);

                            if (num != -1)
                            {
                                str = string.Concat(str, ", ", num.ToString());
                            }
                        }
                    }
                    else
                    {
                        str = string.Concat(str, "handleExternalRef('", match.Groups["FullLink"], "'");
                    }

                    str = string.Concat(str, ");");

                    return str;
                };

            text = Regex.Replace(text,
                                 "(?<Anchor><a[^(href)]*href\\s*=\\s*\")(?<FullLink>(?<Protocol>\\w+:\\/\\/)*(?<Link>[^#\"]*)(\\#(?<TOC>[^\"]*))*)",
                                 match => evaluate(match, Content), RegexHelper.RegexOptsSingleline);

            Evaluate evaluate1 = (match, extendedData) =>
                {
                    string str =
                        extendedData.Keys.First(
                            x => x.Contains(WebUtility.UrlDecode(match.Groups["link"].Value)));
                    var value = new[]
                        {
                            match.Groups["prefix"].Value, "data:", extendedData[str].MimeType, ";base64,",
                            extendedData[str].Content, "\""
                        };

                    return string.Concat(value);
                };

            return Regex.Replace(text, "(?<prefix>(xlink:href|src)\\s*=\\s*\")(?<up_nav>(\\.+/)*)(?<link>[^\"]*)\"",
                                 match => evaluate1(match, ExtraData), RegexHelper.RegexOpts);
        }

        private delegate string Evaluate(Match match, OrderedDictionary<string, EPubEntry> data);
    }
}