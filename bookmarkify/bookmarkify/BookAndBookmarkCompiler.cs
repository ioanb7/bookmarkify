using System;
using System.Collections.Generic;
using System.Linq;

namespace bookmarkify
{
    public class BookAndBookmarkCompiler
    {
        public (Dictionary<string, int> findings, List<int> paragraphsToOutput) Compile(List<string> bookmarks, Models.Book book)
        {
            Dictionary<string, int> findings = new Dictionary<string, int>();
            List<int> paragraphsToOutput = new List<int>();

            // find them
            var uniqueBookmarks = bookmarks.Distinct().ToList();
            for (int bookmarkIndex = 0; bookmarkIndex < uniqueBookmarks.Count; bookmarkIndex++)
            {
                var bookmark = uniqueBookmarks[bookmarkIndex];
                findings[bookmark] = 0;

                for (int paragraphIndex = 0; paragraphIndex < book.Paragraphs.Count; paragraphIndex++)
                {
                    var paragraph = book.Paragraphs[paragraphIndex];
                    foreach (var sentence in paragraph.Sentences)
                    {
                        if (sentence.Contains(bookmark))
                        {
                            findings[bookmark]++;
                            paragraphsToOutput.Add(paragraphIndex);
                        }
                    }
                }
            }

            ReportFindings(findings);
            return (findings, paragraphsToOutput);
        }

        private static void ReportFindings(Dictionary<string, int> findings)
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
