using System.Collections.Generic;

namespace bookmarkify.Models
{
    public class BookmarkParagraphIndexWithMetadata
    {
        public int Index { get; set; }
        public bool IsDirectlyIndented { get; set; }
        public List<BookmarkMetadata> BookmarksFound { get; set; }
    }
}
