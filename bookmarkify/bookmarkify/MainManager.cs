using bookmarkify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace bookmarkify
{
    public class MainManager
    {
        public void Run()
        {
            var mainPathInput = @"H:\Projects\bookmarkify-data";
            var mainPathOutput = @"H:\Projects\bookmarkify-output";

            var txtToListConverter = new TxtToListConverter();

            var bookMarksImporter = new BookMarksImporter(txtToListConverter);
            var bookmarkCollections = bookMarksImporter.Import(mainPathInput);

            var voiceAppTxtToBookConverter = new VoiceAppTxtToBookConverter(txtToListConverter);
            foreach(var bookmarkCollection in bookmarkCollections)
            {
                // get book filepath
                var bookmarkFullpath = bookmarkCollection.FullPath;
                var directoryName = Path.GetFileName(Path.GetDirectoryName(bookmarkFullpath));
                var bookPath = bookmarkCollection.FullPath.Replace(".bmk.txt", ".txt");

                var book = voiceAppTxtToBookConverter.Convert(bookPath);
                var (findings, paragraphsToOutput) = GetFindingsAndParagraphs(bookmarkCollection.Bookmarks, book);
                ReportFindings(findings);
                var fullArrayRange = GetFullArrayRange(paragraphsToOutput);
                OutputAsHtml(mainPathOutput, directoryName, fullArrayRange, book.Paragraphs);

                Debugger.Break();
            }

            var booksImporter = new BooksImporter();
            var books = booksImporter.Import(mainPathInput);

        }

        private void OutputAsHtml(string mainPathOutput, string directoryPath, Dictionary<int, bool> fullArrayRange, List<Paragraph> paragraphs)
        {
            var outputLocation = mainPathOutput + "\\" + directoryPath + ".html";

            string result = @"
<style>
        body {
			margin-left:50px;
			margin-right:50px;
            color: white;
            background-color: black;
            font-family: 'Segoe UI';
            font-weight: 200;
			font-size:1.3em;
            padding: 5px;
			width:960px;
			margin:0 auto;
            margin-top:75px;
        }
		</style>
";
            var lastArrayKey = -100;
            foreach(var array in fullArrayRange)
            {
                var paragraph = paragraphs[array.Key].ToString();
                if (array.Value)
                {
                    paragraph = $"<b>{paragraph}</b>";
                }

                result += $"<p>{paragraph}</p>";
                if (lastArrayKey != array.Key - 1)
                {
                    result += $"<p><i class='missing'>...</i></p>";
                }

                lastArrayKey = array.Key;
            }

            File.WriteAllText(outputLocation, result);
        }

        private Dictionary<int, bool> GetFullArrayRange(List<int> paragraphsToOutput)
        {
            var result = new Dictionary<int, bool>();

            foreach(var paragraphToOutput in paragraphsToOutput.OrderBy(x => x))
            {
                AddFalseIfMissing(result, paragraphToOutput - 1);
                AddFalseIfMissing(result, paragraphToOutput + 1);
                result[paragraphToOutput] = true;
            }

            return result;
        }

        private static void AddFalseIfMissing(Dictionary<int, bool> result, int paragraphToOutput)
        {
            if (!result.ContainsKey(paragraphToOutput))
            {
                result[paragraphToOutput] = false;
            }
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

        private static (Dictionary<string, int> findings, List<int> paragraphsToOutput) GetFindingsAndParagraphs(List<string> bookmarks, Models.Book book)
        {
            Dictionary<string, int> findings = new Dictionary<string, int>();
            //Dictionary<int, string> paragraphsToOutput = new Dictionary<int, string>();
            List<int> paragraphsToOutput = new List<int>();

            // todo: assert uniqueness
            var uniqueBookmarks = bookmarks.Distinct().ToList();

            // find them
            //foreach (var bookmark in uniqueBookmarks)
            for (int bookmarkIndex = 0; bookmarkIndex < uniqueBookmarks.Count; bookmarkIndex++)
            {
                var bookmark = uniqueBookmarks[bookmarkIndex];

                findings[bookmark] = 0;
                //foreach (var paragraph in book.Paragraphs)
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

            return (findings, paragraphsToOutput);
        }
    }
}
