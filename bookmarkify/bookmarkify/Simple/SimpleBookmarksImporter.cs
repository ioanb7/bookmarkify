using bookmarkify.Models;
using System.Collections.Generic;

namespace bookmarkify.Simple
{
    public class SimpleBookmarksImporter : IBookmarksImporter
    {
        public TxtToListConverter TxtToListConverter { get; }

        public SimpleBookmarksImporter(TxtToListConverter txtToListConverter)
        {
            TxtToListConverter = txtToListConverter;
        }

        public BookmarkCollection Import(string path)
        {
            var bookmarksTxt = TxtToListConverter.Convert(path);

            var result = new BookmarkCollection
            {
                FullPath = path,
                Bookmarks = new List<Bookmark>()
            };

            for (int bookmarksTxtIndex = 0; bookmarksTxtIndex < bookmarksTxt.Count - 1; bookmarksTxtIndex += 1)
            {
                var bookmarkTxt = bookmarksTxt[bookmarksTxtIndex];

                BookmarkMetadata metadata = new BookmarkMetadata
                {
                    Colour = HighlightType.Color1,
                    Text = bookmarkTxt
                };

                result.Bookmarks.Add(new Bookmark
                {
                    Text = bookmarkTxt,
                    Metadata = metadata
                });
            }

            return result;
        }
    }
}
