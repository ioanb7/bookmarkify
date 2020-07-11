using bookmarkify.Models;
using bookmarkify.Simple;
using bookmarkify.Utility;
using bookmarkify.Voice;
using System;
using System.Diagnostics;

namespace bookmarkify
{
    public class MainManager
    {
        public void Run(string mainPathInput, string mainPathOutput)
        {
            var logger = new Logger();
            var characterNormaliserConverter = new CharacterNormaliserConverter();
            var txtToListConverter = new TxtToListConverter(characterNormaliserConverter);
            var voiceBookmarkMetadataConverter = new VoiceBookmarkMetadataConverter();
            var voiceBookMarksImporter = new VoiceBookmarksImporter(voiceBookmarkMetadataConverter, txtToListConverter);
            var simpleBookmarksImporter = new SimpleBookmarksImporter(txtToListConverter);
            var bookMetadataImporter = new BookMetadataImporter(logger);
            var txtBookImporter = new TxtBookImporter(txtToListConverter);

            var bookMetadatas = bookMetadataImporter.Import(mainPathInput);
            foreach (var bookMetadata in bookMetadatas)
            {
                logger.Info($"Started converting the book titled '{bookMetadata.BookName}'.");

                var bookImporter = GetBookImporter(txtBookImporter, bookMetadata);
                var book = bookImporter.Import(bookMetadata.BookPath);

                var bookmarksImporter = GetBookmarksImporter(voiceBookMarksImporter, simpleBookmarksImporter, bookMetadata);
                var bookmarkCollection = bookmarksImporter.Import(bookMetadata.BookmarksPath);

                var bookAndBookmarkCompiler = new BookAndBookmarkCompiler(logger);
                var (findings, paragraphsToOutput) = bookAndBookmarkCompiler.Compile(bookmarkCollection.Bookmarks, book);

                var paragraphProximityHelper = new ParagraphProximityHelper();
                var metadatas = paragraphProximityHelper.GetBookmarkParagraphs(paragraphsToOutput, book);

                var htmlExporter = new HtmlExporter();
                var outputLocation = mainPathOutput + "\\" + bookMetadata.BookName + ".html";
                htmlExporter.Output(outputLocation, metadatas, book);

                logger.Info($"Converted the book titled '{bookMetadata.BookName}' and exported it to {outputLocation} successfully.");
            }

            Debugger.Break();
        }

        private IBookImporter GetBookImporter(TxtBookImporter txtToBookConverter, BookMetadata bookMetadata)
        {
            if (bookMetadata.BookImportType == BookImportType.Voice ||
                bookMetadata.BookImportType == BookImportType.Simple)
            {
                if (bookMetadata.BookPath.ToLower().EndsWith(".txt"))
                {
                    return txtToBookConverter;
                }
            }

            throw new NotSupportedException($"Couldn't find book import type for book import {bookMetadata.BookImportType}, path: {bookMetadata.BookPath}.");
        }

        private IBookmarksImporter GetBookmarksImporter(VoiceBookmarksImporter voiceBookMarksImporter, SimpleBookmarksImporter simpleBookmarksImporter, BookMetadata bookMetadata)
        {
            if (bookMetadata.BookImportType == BookImportType.Voice)
            {
                return voiceBookMarksImporter;
            }

            if (bookMetadata.BookImportType == BookImportType.Simple)
            {
                return simpleBookmarksImporter;
            }

            throw new NotSupportedException($"Couldn't find book importer for book import type {bookMetadata.BookImportType}.");
        }
    }
}
