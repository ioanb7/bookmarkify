using System.Collections.Generic;

namespace bookmarkify.Models
{
    public class Book
    {
        public List<Paragraph> Paragraphs { get; set; }
        public string Path { get; set; }
    }
}
