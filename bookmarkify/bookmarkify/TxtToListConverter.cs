using bookmarkify.Utility;
using System.Collections.Generic;
using System.IO;

namespace bookmarkify
{
    public class TxtToListConverter
    {
        public TxtToListConverter(CharacterNormaliserConverter characterNormaliserConverter)
        {
            CharacterNormaliserConverter = characterNormaliserConverter;
        }

        public CharacterNormaliserConverter CharacterNormaliserConverter { get; }

        public void Convert(List<string> result, string path)
        {
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                result.Add(CharacterNormaliserConverter.Normalise(line));
            }
        }

        public List<string> Convert(string path)
        {
            var result = new List<string>();
            Convert(result, path);
            return result;
        }
    }
}
