using System;
using System.Threading.Tasks;
using MyBookLibrary.Helpers;
using Windows.Storage;

namespace MyBookLibrary.BookReader
{
    public class EPubItemBuilder
    {
        public async Task<EPubItem> GetItem(StorageFile itemFile)
        {
            return await PopulateItem(new EPubItem(), itemFile);
        }

        public async Task<EPubItem> PopulateItem(EPubItem item, StorageFile itemFile)
        {
            EPub ePub = await (new EPubBuilder()).GetEPub(itemFile);

            item.Title = (ePub.Title.Count > 0 ? ePub.Title[0] : itemFile.DisplayName);
            item.Author = (ePub.Creator.Count > 0 ? ePub.Creator[0] : "Unknown");
            item.ItemId = (string.IsNullOrEmpty(ePub.Uuid) ? ePub.GetTitle() : ePub.Uuid);

            if (ePub.Publisher.Count > 0)
                item.Publisher = ePub.Publisher[0];

            DateTime dateTime;
            if (ePub.Date.Count > 0 && DateTime.TryParse(ePub.Date[0].Date, out dateTime))
                item.PublishingDate = dateTime;

            item.SetUnderlyingToc(ePub.TableOfContents);

            if (ePub.CoverImage != null)
            {
                item.CoverImageFileName = string.Concat(item.ItemId,
                                                        ImageHelper.GetExtensionForMimeType(ePub.CoverImage.MimeType));
                item.CoverImage = Convert.FromBase64String(ePub.CoverImage.Content.Replace(' ', '+'));
            }

            return item;
        }
    }
}