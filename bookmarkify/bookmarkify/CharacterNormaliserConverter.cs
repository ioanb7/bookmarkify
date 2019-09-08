using System;
using System.Collections.Generic;
using System.Text;

namespace bookmarkify
{
    public class CharacterNormaliserConverter
    {
        public string Normalise(string input)
        {
            var inputCopy = input.ToString();
            if (inputCopy.IndexOf('\u2013') > -1) inputCopy = inputCopy.Replace('\u2013', '-'); // en dash
            if (inputCopy.IndexOf('\u2014') > -1) inputCopy = inputCopy.Replace('\u2014', '-'); // em dash
            if (inputCopy.IndexOf('\u2015') > -1) inputCopy = inputCopy.Replace('\u2015', '-'); // horizontal bar
            if (inputCopy.IndexOf('\u2017') > -1) inputCopy = inputCopy.Replace('\u2017', '_'); // double low line
            if (inputCopy.IndexOf('\u2018') > -1) inputCopy = inputCopy.Replace('\u2018', '\''); // left single quotation mark
            if (inputCopy.IndexOf('\u2019') > -1) inputCopy = inputCopy.Replace('\u2019', '\''); // right single quotation mark
            if (inputCopy.IndexOf('\u201a') > -1) inputCopy = inputCopy.Replace('\u201a', ','); // single low-9 quotation mark
            if (inputCopy.IndexOf('\u201b') > -1) inputCopy = inputCopy.Replace('\u201b', '\''); // single high-reversed-9 quotation mark
            if (inputCopy.IndexOf('\u201c') > -1) inputCopy = inputCopy.Replace('\u201c', '\"'); // left double quotation mark
            if (inputCopy.IndexOf('\u201d') > -1) inputCopy = inputCopy.Replace('\u201d', '\"'); // right double quotation mark
            if (inputCopy.IndexOf('\u201e') > -1) inputCopy = inputCopy.Replace('\u201e', '\"'); // double low-9 quotation mark
            if (inputCopy.IndexOf('\u2026') > -1) inputCopy = inputCopy.Replace("\u2026", "..."); // horizontal ellipsis
            if (inputCopy.IndexOf('\u2032') > -1) inputCopy = inputCopy.Replace('\u2032', '\''); // prime
            if (inputCopy.IndexOf('\u2033') > -1) inputCopy = inputCopy.Replace('\u2033', '\"');
            inputCopy = inputCopy.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"');

            inputCopy = inputCopy.Replace("”", "\"").Replace("“", "\"");
            return inputCopy;
        }
    }
}
