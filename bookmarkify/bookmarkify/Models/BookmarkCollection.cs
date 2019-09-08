using System.Collections.Generic;

namespace bookmarkify.Models
{
    public class BookmarkCollection
    {
        public List<Bookmark> Bookmarks { get; set; }
        public string FullPath { get; set; }
    }
}
