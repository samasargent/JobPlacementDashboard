using System;
using System.Collections.Generic;
using System.Linq;

namespace TheatreCMS3.Helpers
{
    public static class TextHelpers
    {

        public static string LimitWords(string value, int maxWords)
        {
            // if string is null or empty, return string value
            if (string.IsNullOrEmpty(value)) return value;

            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < value.Length && char.IsWhiteSpace(value[index]))
                index++;

            while (index < value.Length)
            {
                // check if current char is part of a word
                while (index < value.Length && !char.IsWhiteSpace(value[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < value.Length && char.IsWhiteSpace(value[index]))
                    index++;
            }

            var newWords = string.Empty;

            // if max words is set to 0 or less than 0, return string value
            if (maxWords < 0 || maxWords == 0) return value;

            // if word count is greater than max words, get max words
            if (wordCount > maxWords)
            {
                // Create list based on number of words (Take(maxwords))
                // Split on spaces, using RemoveEmptyEntries to ignore spaces in collecting words
                List<string> myWordsList = value.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Take(maxWords).ToList();
                // Create string of words from the list, seperated by spaces
                string pickedWords = string.Join(" ", myWordsList);
                // Add ellipses to the end of the string
                newWords = pickedWords + "...";            }
            else
            {
                newWords = value;
            }

            return newWords;
        }
    }
}
