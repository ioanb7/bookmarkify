using bookmarkify.Models;
using Ganss.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace bookmarkify
{
    public class BookMetadataImporter
    {
        public ILogger Logger { get; }

        public BookMetadataImporter(ILogger logger)
        {
            Logger = logger;
        }

        public List<BookMetadata> Import(string path)
        {
            var result = new List<BookMetadata>();
            if (path[path.Length - 1] == '\\')
            {
                throw new InvalidOperationException($"Path {path} can't end with trailing slash.");
            }

            var bookmarkFiles = Glob.Expand(path + @"\**\*.bmk.txt");
            foreach (var bookmarkFile in bookmarkFiles)
            {
                var folder = Path.GetDirectoryName(bookmarkFile.FullName);

                // check for the txt book 
                var txtBookPath = bookmarkFile.FullName.Replace(".bmk.txt", ".txt");
                if (!File.Exists(txtBookPath))
                {
                    Logger.Warn($"Couldn't import book for bookmark file {bookmarkFile.FullName}");
                    continue;
                }

                result.Add(new BookMetadata
                {
                    BookImportType = GetBookImportType(folder),
                    BookmarksPath = bookmarkFile.FullName,
                    BookPath = txtBookPath,
                    BookName = Path.GetFileName(Path.GetDirectoryName(bookmarkFile.FullName))
                });
            }

            return result;
        }

        private static BookImportType GetBookImportType(string folder)
        {
            var bookImportType = BookImportType.Voice;
            if (IsSimple(folder))
            {
                bookImportType = BookImportType.Simple;
            }

            return bookImportType;
        }

        private static bool IsEpub(string folderPath)
        {
            return GetFileByExtensionFromFolder(folderPath, "*.epub").Any();
        }

        private static bool IsSimple(string folderPath)
        {
            return GetFileByExtensionFromFolder(folderPath, ".simple").Any();
        }

        private static IEnumerable<IFileSystemInfo> GetFileByExtensionFromFolder(string folderPath, string filePattern)
        {
            return Glob.Expand(folderPath + @"\" + filePattern);
        }
    }
}
