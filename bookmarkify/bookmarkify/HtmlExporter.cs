using bookmarkify.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bookmarkify
{
    public class HtmlExporter
    {
        public string OutputFilePath { get; set; }
        public Book Book { get; set; }
        public List<BookmarkParagraphIndexWithMetadata> Metadatas { get; set; }

        // sorted list
        public void Output(string outputFilePath, List<BookmarkParagraphIndexWithMetadata> metadatas, Book book)
        {
            OutputFilePath = outputFilePath;
            Book = book;
            Metadatas = metadatas;

            if (!metadatas.Any())
            {
                throw new InvalidOperationException($"Missing metadatas for {OutputFilePath}.");
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
            File.WriteAllText(outputFilePath, result);
        }

        private string GetParagraphWrapped(BookmarkParagraphIndexWithMetadata metadata, int paragraphIndex)
        {
            var paragraph = Book.Paragraphs[paragraphIndex].ToString();

            if (metadata.BookmarksFound != null)
            {
                foreach (var bookmarkFound in metadata.BookmarksFound)
                {
                    var higlightTypeInt = (int)bookmarkFound.Colour;
                    var text = bookmarkFound.Text;
                    paragraph = paragraph.Replace(text, $"<span class='bookmark_selection highlight_{higlightTypeInt}''>{text}</span>");
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
}

.missing {
    font-weight:0.5em;
}

.highlight_0 {
    background-color: gray;
}
.highlight_1 {
    background-color: red;
}
.highlight_2 {
    background-color: yellow;
}
.highlight_3 {
    background-color: pink;
}
.highlight_4 {
    background-color: blue;
}

		</style>
" + result;
            return result;
        }
    }
}
