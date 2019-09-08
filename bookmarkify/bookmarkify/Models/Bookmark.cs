using System;
using System.Collections.Generic;
using System.Text;

namespace bookmarkify.Models
{
    public class Bookmark
    {
        public string Text { get; set; }
        public BookmarkMetadata Metadata { get; set; }
    }
}
