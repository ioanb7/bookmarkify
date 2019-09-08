﻿using bookmarkify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bookmarkify
{
    public class ParagraphProximityHelper
    {
        public List<BookmarkIndexWithMetadata> GetFullArrayRange(List<(int, string)> paragraphsToOutput, Book book)
        {
            var resultPre = new Dictionary<int, bool>();
            var resultPreText = new Dictionary<int, List<string>>();

            foreach (var paragraphToOutputTuple in paragraphsToOutput.OrderBy(x => x))
            {
                var paragraphToOutput = paragraphToOutputTuple.Item1;
                AddFalseIfMissing(resultPre, paragraphToOutput - 1, book);
                AddFalseIfMissing(resultPre, paragraphToOutput + 1, book);
                resultPre[paragraphToOutput] = true;

                if (!resultPreText.ContainsKey(paragraphToOutput))
                    resultPreText[paragraphToOutput] = new List<string>();

                resultPreText[paragraphToOutput].Add(paragraphToOutputTuple.Item2);
            }

            var result = resultPre.Select(x => new BookmarkIndexWithMetadata
            {
                Index = x.Key,
                IsDirectlyIndented = x.Value,
                TextFound = !resultPreText.ContainsKey(x.Key) ? null : resultPreText[x.Key]
            }).OrderBy(x => x.Index).ToList();

            foreach (var metadata in result)
            {
                Console.WriteLine($"Got metadata for line: {metadata.Index} with {metadata.IsDirectlyIndented.ToString()}");
            }
            return result;
        }

        private static void AddFalseIfMissing(Dictionary<int, bool> result, int paragraphToOutput, Book book)
        {
            if (paragraphToOutput > book.Paragraphs.Count - 1)
            {
                return;
            }

            if (paragraphToOutput < 0)
            {
                return;
            }

            if (!result.ContainsKey(paragraphToOutput))
            {
                result[paragraphToOutput] = false;
            }
        }
    }
}