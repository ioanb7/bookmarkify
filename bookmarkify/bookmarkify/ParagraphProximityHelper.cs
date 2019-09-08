using bookmarkify.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bookmarkify
{
    public class ParagraphProximityHelper
    {
        public List<BookmarkParagraphIndexWithMetadata> GetBookmarkParagraphs(List<(int paragraphIndex, Bookmark bookmark)> paragraphsToOutput, Book book)
        {
            var results = new Dictionary<int, bool>();
            var resultsMetadata = new Dictionary<int, List<BookmarkMetadata>>();

            foreach (var paragraphToOutputTuple in paragraphsToOutput.OrderBy(x => x.paragraphIndex))
            {
                var paragraphToOutput = paragraphToOutputTuple.paragraphIndex;
                AddContextParagraphs(book, results, paragraphToOutput);

                results[paragraphToOutput] = true;

                if (!resultsMetadata.ContainsKey(paragraphToOutput))
                {
                    resultsMetadata[paragraphToOutput] = new List<BookmarkMetadata>();
                }
                resultsMetadata[paragraphToOutput].Add(new BookmarkMetadata
                {
                    Text = paragraphToOutputTuple.bookmark.Text,
                    Colour = paragraphToOutputTuple.bookmark.Metadata.Colour
                });
            }

            var result = results.Select(x => new BookmarkParagraphIndexWithMetadata
            {
                Index = x.Key,
                IsDirectlyIndented = x.Value,
                BookmarksFound = resultsMetadata.ContainsKey(x.Key) ? resultsMetadata[x.Key] : null
            }).OrderBy(x => x.Index).ToList();
            
            return result;
        }

        private static void AddContextParagraphs(Book book, Dictionary<int, bool> resultPre, int paragraphToOutput)
        {
            for (int i = 1; i < 3; i++)
            {
                AddFalseIfMissing(resultPre, paragraphToOutput - i, book);
                AddFalseIfMissing(resultPre, paragraphToOutput + i, book);
            }
        }

        private static void AddFalseIfMissing(Dictionary<int, bool> result, int paragraphToOutput, Book book)
        {
            if (paragraphToOutput > book.Paragraphs.Count - 1)
            {
                return;
            }

            if (paragraphToOutput < 0)
            {
                return;
            }

            if (!result.ContainsKey(paragraphToOutput))
            {
                result[paragraphToOutput] = false;
            }
        }
    }
}
