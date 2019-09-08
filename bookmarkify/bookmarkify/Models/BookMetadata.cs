namespace bookmarkify.Models
{
    public class BookMetadata
    {
        public string BookPath { get; set; }
        public string BookName { get; set; }
        public string BookmarksPath { get; set; }
        public BookImportType BookImportType { get; set; }
    }
}
