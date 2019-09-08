using bookmarkify.Models;

namespace bookmarkify
{
    public interface IBookImporter
    {
        TxtToListConverter TxtToListConverter { get; }

        Book Import(string path);
    }
}