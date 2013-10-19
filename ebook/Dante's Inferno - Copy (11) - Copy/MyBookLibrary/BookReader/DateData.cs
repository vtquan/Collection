namespace MyBookLibrary.BookReader
{
    public class DateData
    {
        public DateData(string type, string date)
        {
            Type = type;
            Date = date;
        }

        public string Date { get; private set; }

        public string Type { get; private set; }
    }
}