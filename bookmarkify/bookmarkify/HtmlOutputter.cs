using bookmarkify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bookmarkify
{
    public class HtmlOutputter
    {
        public string FilePath { get; set; }
        public Book Book { get; set; }
        public List<BookmarkIndexWithMetadata> Metadatas { get; set; }

        // sorted list
        public void Output(string filePath, List<BookmarkIndexWithMetadata> metadatas, Book book)
        {
            FilePath = filePath;
            Book = book;
            Metadatas = metadatas;

            if (!metadatas.Any())
            {
                throw new InvalidOperationException("Missing metadatas.");
            }

            string result = "";
            var lastArrayKey = metadatas.First().Index - 1;
            foreach (var array in metadatas.OrderBy(x => x.Index))
            {
                var paragraphIndex = array.Index;
                var resultTemp = "";
                resultTemp += AddMissing(lastArrayKey, paragraphIndex);
                resultTemp += GetParagraphWrapped(array, paragraphIndex);

                result += resultTemp;
                lastArrayKey = paragraphIndex;
            }

            result = AddBeginningOfHtmlFile(result);
            File.WriteAllText(filePath, result);
        }

        private string GetParagraphWrapped(BookmarkIndexWithMetadata metadata, int paragraphIndex)
        {
            var paragraph = Book.Paragraphs[paragraphIndex].ToString();

            if (metadata.TextFound != null)
            {
                foreach (var textFound in metadata.TextFound)
                {
                    paragraph = paragraph.Replace(textFound, $"<span class='bookmark_selection'>{textFound}</span>");
                }
            }

            if (metadata.IsDirectlyIndented)
            {
                paragraph = $"<span class='directly_indented'>{paragraph}</span>";
            }

            //return $"<p>{paragraphIndex}: {paragraph}</p>"; // DEBUG
            return $"<p>{paragraph}</p>";
        }

        private string AddMissing(int lastArrayKey, int paragraphIndex)
        {
            if (lastArrayKey != paragraphIndex - 1)
            {
                return $"<p><span class='missing'>...</span></p>";
            }

            return "";
        }

        private string AddBeginningOfHtmlFile(string result)
        {
            result = @"
  <meta charset='UTF-8'>
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

.bookmark_selection {
    font-style: italic;
    background-color: red;
}

.missing {
    font-weight:0.5em;
}

		</style>
" + result;
            return result;
        }
    }
}
