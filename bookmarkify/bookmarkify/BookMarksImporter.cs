using bookmarkify.Models;
using Ganss.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace bookmarkify
{
    public class BookMarksImporter
    {
        public VoiceBookmarkMetadataConverter VoiceBookmarkMetadataConverter { get; }
        public TxtToListConverter TxtToListConverter { get; }

        public BookMarksImporter(VoiceBookmarkMetadataConverter voiceBookmarkMetadataConverter, TxtToListConverter txtToListConverter)
        {
            VoiceBookmarkMetadataConverter = voiceBookmarkMetadataConverter;
            TxtToListConverter = txtToListConverter;
        }

        public List<BookmarkCollection> Import(string path)
        {
            var result = new List<BookmarkCollection>();
            if (path[path.Length-1] == '\\')
            {
                throw new InvalidOperationException("can't end with trailing slash.");
            }

            var bookmarkFiles = Glob.Expand(path + @"\**\*.bmk.txt");
            foreach (var bookmarkFile in bookmarkFiles)
            {
                var folder = Path.GetDirectoryName(bookmarkFile.FullName);
                var epubsInFolder = Glob.Expand(path + @"\*.epub");
                if (!epubsInFolder.Any())
                {
                    // check for the txt book 
                    var txtBookPath = bookmarkFile.FullName.Replace(".bmk.txt", ".txt");
                    if (!File.Exists(txtBookPath))
                    {
                        continue;
                    }
                }

                BookmarkCollection bookmarkCollection = ExtractBookmarkCollection(bookmarkFile);
                result.Add(bookmarkCollection);
            }

            return result;
        }

        private BookmarkCollection ExtractBookmarkCollection(IFileSystemInfo bookmarkFile)
        {
            var bookmarksTxt = TxtToListConverter.Convert(bookmarkFile.FullName);
            bookmarksTxt.RemoveAll(x => BadTexts(x));

            var result = new BookmarkCollection
            {
                FullPath = bookmarkFile.FullName,
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
