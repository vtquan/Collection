using System.ComponentModel;

namespace MyBookLibrary.Core
{
    public class TableOfContensEntry : INotifyPropertyChanged
    {
        private string _section;

        private string _sectionName;

        public string Section
        {
            get { return _section; }
            set
            {
                _section = value;
                RaisePropertyChanged("Section");
            }
        }

        public string SectionName
        {
            get { return _sectionName; }
            set
            {
                _sectionName = value;
                RaisePropertyChanged("SectionName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}