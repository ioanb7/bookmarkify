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

        public List<string> Convert(string path)
        {
            var lines = File.ReadAllLines(path);

            var result = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                result.Add(CharacterNormaliserConverter.Normalise(line));
            }

            return result;
        }
    }
}
