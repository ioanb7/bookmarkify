using bookmarkify.Models;
using System.Collections.Generic;
using System.Linq;

namespace bookmarkify
{
    public class VoiceAppTxtToBookConverter
    {
        public VoiceAppTxtToBookConverter(TxtToListConverter txtToListConverter)
        {
            TxtToListConverter = txtToListConverter;
        }

        public TxtToListConverter TxtToListConverter { get; }

        public Book Convert(string path)
        {
            var paragraphs = TxtToListConverter.Convert(path);
            return new Book
            {
                Paragraphs = paragraphs.Select(x => new Paragraph { Sentences = new List<string> { x } }).ToList()
            };
        }
    }
}
