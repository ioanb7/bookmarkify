using bookmarkify.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace bookmarkify
{
    public class MainManager
    {
        public void Run(string mainPathInput, string mainPathOutput)
        {
            var characterNormaliserConverter = new CharacterNormaliserConverter();
            var txtToListConverter = new TxtToListConverter(characterNormaliserConverter);

            var voiceBookmarkMetadataConverter = new VoiceBookmarkMetadataConverter();
            var voiceBookMarksImporter = new VoiceBookMarksImporter(voiceBookmarkMetadataConverter, txtToListConverter);

            var simpleBookmarksImporter = new SimpleBookmarksImporter(txtToListConverter);

            var bookMetadataImporter = new BookMetadataImporter();
            var bookMetadatas = bookMetadataImporter.Import(mainPathInput);

            var txtToBookConverter = new TxtToBookConverter(txtToListConverter);
            foreach(var bookMetadata in bookMetadatas)
            {
                var book = ImportBook(txtToBookConverter, bookMetadata);

                var bookAndBookmarkCompiler = new BookAndBookmarkCompiler();
                var bookmarkCollection = GetBookmarkCollection(voiceBookMarksImporter, simpleBookmarksImporter, bookMetadata);
                var (findings, paragraphsToOutput) = bookAndBookmarkCompiler.Compile(bookmarkCollection.Bookmarks, book);

                var paragraphProximityHelper = new ParagraphProximityHelper();
                var metadatas = paragraphProximityHelper.GetFullArrayRange(paragraphsToOutput, book);

                var outputLocation = mainPathOutput + "\\" + bookMetadata.BookName + ".html";
                var htmlOutputter = new HtmlOutputter();
                htmlOutputter.Output(outputLocation, metadatas, book);
            }

            Debugger.Break();
        }

        private static Book ImportBook(TxtToBookConverter txtToBookConverter, BookMetadata bookMetadata)
        {
            if (bookMetadata.BookImportType == BookImportType.Voice ||
                bookMetadata.BookImportType == BookImportType.Simple)
            {
                if (bookMetadata.BookPath.ToLower().EndsWith(".txt"))
                {
                    return txtToBookConverter.Import(bookMetadata.BookPath);
                }
            }

            throw new InvalidOperationException($"Couldn't find book import type for book import {bookMetadata.BookImportType}, path: {bookMetadata.BookPath}.");
        }

        private static BookmarkCollection GetBookmarkCollection(
            VoiceBookMarksImporter voiceBookMarksImporter,
            SimpleBookmarksImporter simpleBookmarksImporter,
            BookMetadata bookMetadata)
        {
            if (bookMetadata.BookImportType == BookImportType.Voice)
            {
                return voiceBookMarksImporter.Import(bookMetadata.BookmarksPath);
            }

            if (bookMetadata.BookImportType == BookImportType.Simple)
            {
                return simpleBookmarksImporter.Import(bookMetadata.BookmarksPath);
            }

            throw new InvalidOperationException($"Couldn't find book import type for bookmark import {bookMetadata.BookImportType}.");
        }
    }
}
