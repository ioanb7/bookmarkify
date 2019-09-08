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
        public TxtToListConverter TxtToListConverter { get; }

        public BookMarksImporter(TxtToListConverter txtToListConverter)
        {
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
            bookmarksTxt.RemoveAll(x => x.StartsWith("*** ["));

            var bookmarks = bookmarksTxt.Select(x => new Bookmark
            {
                Text = x
            }).ToList();

            return new BookmarkCollection
            {
                FullPath = bookmarkFile.FullName,
                Bookmarks = bookmarks
            };
        }
    }
}
