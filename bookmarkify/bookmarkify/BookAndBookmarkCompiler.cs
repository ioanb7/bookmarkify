using bookmarkify.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bookmarkify
{
    public class BookAndBookmarkCompiler
    {
        public (Dictionary<Bookmark, int> findings, List<(int, Bookmark)> paragraphsToOutput) Compile(List<Bookmark> bookmarks, Models.Book book)
        {
            Dictionary<Bookmark, int> findings = new Dictionary<Bookmark, int>();
            List<(int, Bookmark)> paragraphsToOutput = new List<(int, Bookmark)>();

            // find them
            for (int bookmarkIndex = 0; bookmarkIndex < bookmarks.Count; bookmarkIndex++)
            {
                var bookmark = bookmarks[bookmarkIndex];
                findings[bookmark] = 0;

                for (int paragraphIndex = 0; paragraphIndex < book.Paragraphs.Count; paragraphIndex++)
                {
                    var paragraph = book.Paragraphs[paragraphIndex];
                    foreach (var sentence in paragraph.Sentences)
                    {
                        if (sentence.Contains(bookmark.Text))
                        {
                            findings[bookmark]++;
                            paragraphsToOutput.Add((paragraphIndex, bookmark));
                        }
                    }
                }
            }

            ReportFindings(findings);
            return (findings, paragraphsToOutput);
        }

        private static void ReportFindings(Dictionary<Bookmark, int> findings)
        {
            foreach (var finding in findings.Where(x => x.Value == 0))
            {
                Console.WriteLine($"Couldn't find bookmark {finding.Key}");
            }

            foreach (var finding in findings.Where(x => x.Value > 1))
            {
                Console.WriteLine($"Found bookmark multiple times: {finding.Key}");
            }
        }
    }
}
