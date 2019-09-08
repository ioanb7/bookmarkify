using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bookmarkify
{
    public class TxtToListConverter
    {
        public void Convert(List<string> result, string path)
        {
            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                result.Add(line);
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
