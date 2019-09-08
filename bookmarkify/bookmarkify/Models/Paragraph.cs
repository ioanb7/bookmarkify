using System;
using System.Collections.Generic;
using System.Text;

namespace bookmarkify.Models
{
    public class Paragraph
    {
        public List<string> Sentences { get; set; }

        public override string ToString()
        {
            return string.Join("", Sentences);
        }
    }
}
