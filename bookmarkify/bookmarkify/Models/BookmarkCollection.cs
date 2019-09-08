using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
namespace bookmarkify.Models
{
    public class BookmarkCollection
    {
        public List<string> Bookmarks { get; set; }
        public string FullPath { get; set; }
    }
}
