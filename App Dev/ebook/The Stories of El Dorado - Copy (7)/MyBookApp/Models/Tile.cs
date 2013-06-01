namespace MyBookApp.Models
{
    internal class Tile : MenuItem
    {
        public Tile()
        {
            RowSpan = 1;
        }

        public string ImageUrl { get; set; }

        public string LinkUrl { get; set; }

        public int RowSpan { get; set; }
    }
}