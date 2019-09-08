using bookmarkify.Models;

namespace bookmarkify
{
    public interface IBookmarksImporter
    {
        BookmarkCollection Import(string path);
    }
}