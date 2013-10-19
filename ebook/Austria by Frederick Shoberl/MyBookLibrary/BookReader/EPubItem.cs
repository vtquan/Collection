using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using MyBookLibrary.Core;
using Windows.Storage;

namespace MyBookLibrary.BookReader
{
    public class EPubItem : INotifyPropertyChanged
    {
        private string _author;
        private string _coverImageFileName;
        private EPub _epub;
        private string _publisher;
        private DateTime? _publishingDate;
        private string _title;
        private List<NavigationPoint> _underlyingToc;

        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                RaisePropertyChanged("Author");
            }
        }

        public Byte[] CoverImage { get; set; }

        public string CoverImageFileName
        {
            get { return _coverImageFileName; }
            set
            {
                _coverImageFileName = value;
                RaisePropertyChanged("CoverImageFileName");
            }
        }

        public string ItemId { get; set; }

        public string Publisher
        {
            get { return _publisher; }
            set
            {
                _publisher = value;
                RaisePropertyChanged("Publisher");
            }
        }

        public DateTime? PublishingDate
        {
            get { return _publishingDate; }
            set
            {
                _publishingDate = value;
                RaisePropertyChanged("PublishingDate");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<string> GetContent(string htmlTemplate)
        {
            EPub ePub = await GetEPub();

            return ePub.GetHtml(htmlTemplate);
        }

        public async Task<string> GetContentForSection(int section)
        {
            EPub ePub = await GetEPub();

            return await ePub.GetHtmlForSection(section);
        }

        private async Task<EPub> GetEPub()
        {
            if (_epub == null)
            {
                StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ItemId));

                _epub = await (new EPubBuilder()).GetEPub(storageFile);

                if (_epub != null) SetUnderlyingToc(_epub.TableOfContents);
            }

            return _epub;
        }

        public Task<ObservableCollection<TableOfContensEntry>> GetTableOfContens()
        {
            var observableCollection = new ObservableCollection<TableOfContensEntry>();

            foreach (NavigationPoint navPoint in _underlyingToc)
            {
                observableCollection.Add(new TableOfContensEntry {SectionName = navPoint.Title, Section = ""});
            }

            return Task.FromResult(observableCollection);
        }

        public void SetUnderlyingToc(List<NavigationPoint> toc)
        {
            _underlyingToc = toc;
        }

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}