using bookmarkify.Models;
using System.Collections.Generic;
using System.Linq;

namespace bookmarkify
{
    public class TxtBookImporter : IBookImporter
    {
        public TxtBookImporter(TxtToListConverter txtToListConverter)
        {
            TxtToListConverter = txtToListConverter;
        }

        public TxtToListConverter TxtToListConverter { get; }

        public Book Import(string path)
        {
            var paragraphs = TxtToListConverter.Convert(path);
            return new Book
            {
                Paragraphs = paragraphs.Select(x => new Paragraph { Sentences = new List<string> { x } }).ToList(),
                Path = path
            };
        }
    }
}
