namespace bookmarkify.Models
{
    public class Bookmark
    {
        public string Text { get; set; }
        public BookmarkMetadata Metadata { get; set; }

        public override string ToString()
        {
            return $"Colour {Metadata.Colour.ToString()}: {Text}";
        }
    }
}
