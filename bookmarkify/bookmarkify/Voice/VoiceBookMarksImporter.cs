using bookmarkify.Models;
using System;
using System.Collections.Generic;

namespace bookmarkify.Voice
{
    public class VoiceBookmarksImporter : IBookmarksImporter
    {
        public VoiceBookmarkMetadataConverter VoiceBookmarkMetadataConverter { get; }
        public TxtToListConverter TxtToListConverter { get; }

        public VoiceBookmarksImporter(VoiceBookmarkMetadataConverter voiceBookmarkMetadataConverter, TxtToListConverter txtToListConverter)
        {
            VoiceBookmarkMetadataConverter = voiceBookmarkMetadataConverter;
            TxtToListConverter = txtToListConverter;
        }

        public BookmarkCollection Import(string path)
        {
            var bookmarksTxt = TxtToListConverter.Convert(path);
            bookmarksTxt.RemoveAll(x => BadTexts(x));

            var result = new BookmarkCollection
            {
                FullPath = path,
                Bookmarks = new List<Bookmark>()
            };

            for (int bookmarksTxtIndex = 0; bookmarksTxtIndex < bookmarksTxt.Count - 1; bookmarksTxtIndex += 2)
            {
                var bookmarkTxt = bookmarksTxt[bookmarksTxtIndex];
                if (!IsMetadataLine(bookmarkTxt))
                {
                    throw new InvalidOperationException($"{bookmarkTxt} wasn't metadata.");
                }

                BookmarkMetadata metadata = VoiceBookmarkMetadataConverter.Convert(bookmarkTxt);

                bookmarkTxt = bookmarksTxt[bookmarksTxtIndex+1];
                result.Bookmarks.Add(new Bookmark
                {
                    Text = bookmarkTxt,
                    Metadata = metadata
                });
            }

            return result;
        }

        private static bool IsMetadataLine(string x)
        {
            return x.StartsWith("*** [");
        }

        private static bool BadTexts(string x)
        {
            return x == "Last search start" ||
                x.StartsWith("@Voice bookmarks exported") ||
                x.StartsWith("xxHash: ");
        }
    }
}
