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
            var bookMarksImporter = new BookMarksImporter(voiceBookmarkMetadataConverter, txtToListConverter);
            var bookmarkCollections = bookMarksImporter.Import(mainPathInput);

            var voiceAppTxtToBookConverter = new VoiceAppTxtToBookConverter(txtToListConverter);
            foreach(var bookmarkCollection in bookmarkCollections)
            {
                // get book filepath
                var bookmarkFullpath = bookmarkCollection.FullPath;
                var directoryName = Path.GetFileName(Path.GetDirectoryName(bookmarkFullpath));
                var bookPath = bookmarkCollection.FullPath.Replace(".bmk.txt", ".txt");

                var book = voiceAppTxtToBookConverter.Convert(bookPath);

                var bookAndBookmarkCompiler = new BookAndBookmarkCompiler();
                var (findings, paragraphsToOutput) = bookAndBookmarkCompiler.Compile(bookmarkCollection.Bookmarks, book);

                var paragraphProximityHelper = new ParagraphProximityHelper();
                var metadatas = paragraphProximityHelper.GetFullArrayRange(paragraphsToOutput, book);

                var outputLocation = mainPathOutput + "\\" + directoryName + ".html";
                var htmlOutputter = new HtmlOutputter();
                htmlOutputter.Output(outputLocation, metadatas, book);
            }

            Debugger.Break();
        }
    }
}
