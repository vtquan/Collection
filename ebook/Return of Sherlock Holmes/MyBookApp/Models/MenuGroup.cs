using System.Collections.Generic;

namespace MyBookApp.Models
{
    internal class MenuGroup
    {
        public string Title { get; set; }

        public List<MenuItem> Items { get; set; }
    }
}