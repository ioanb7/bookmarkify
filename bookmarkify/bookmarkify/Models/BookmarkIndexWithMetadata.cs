using System;
using System.Collections.Generic;
using System.Text;

namespace bookmarkify.Models
{
    public class BookmarkIndexWithMetadata
    {
        public int Index { get; set; }
        public bool IsDirectlyIndented { get; set; }
        public int Colour { get; set; }
        public List<string> TextFound { get; set; }
    }
}
