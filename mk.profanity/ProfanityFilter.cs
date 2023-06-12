using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace mk.profanity
{
    public class ProfanityFilter
    {
        private List<string> badWords = Storage.words;
        private List<string> allowWords = new List<string> { };
        private bool useLevenshteinDistance;

        /// <summary>
        /// Creates a new instance of the ProfanityFilter class with default options.
        /// </summary>
        public ProfanityFilter()
        {
            // Empty constructor
        }

        /// <summary>
        /// Creates a new instance of the ProfanityFilter class with custom options.
        /// </summary>
        /// <param name="configure">A delegate that takes a ProfanityFilterOptions object as input and sets the custom options.</param>
        public ProfanityFilter(Action<ProfanityFilterOptions> configure)
        {
            var options = new ProfanityFilterOptions();
            configure(options);

            if (options.UseBadWordsList != null)
                badWords = options.UseBadWordsList;

            if (options.UseAllowWordList != null)
                allowWords = options.UseAllowWordList;


            if (options.UseAppendBadWordsList != null)
                badWords.AddRange(options.UseAppendBadWordsList);

            useLevenshteinDistance = options.UseLevenshtein;
        }

        /// <summary>
        /// Represents a profanity in text and its position and length.
        /// </summary>
        public class ProfanityLocation
        {
            /// <summary>
            /// The starting position of the bad word in the text.
            /// </summary>
            public int Position { get; set; }

            /// <summary>
            /// The length of the bad word.
            /// </summary>
            public int Length { get; set; }
        }

        /// <summary>
        /// Finds all occurrences of bad words in the input text and their positions and lengths.
        /// </summary>
        /// <param name="text">The input text to search for bad words.</param>
        /// <returns>A dictionary with the bad words as keys and lists of BadWord objects as values, representing their positions and lengths.</returns>
        public Dictionary<string, List<ProfanityLocation>> FindProfanities(string text)
        {
            var regex = new Regex(@"\W+"); // Split by non-letter characters
            string[] words = regex.Split(text);

            // Find all bad words in the text
            Dictionary<string, List<ProfanityLocation>> foundBadWords = new Dictionary<string, List<ProfanityLocation>>();
            int index = 0;
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                int wordIndex = text.IndexOf(word, index, StringComparison.OrdinalIgnoreCase);
                if (wordIndex >= 0)
                {
                    index = wordIndex + word.Length;
                }

                if (!allowWords.Contains(word) && badWords.Contains(word.ToLower()))
                {
                    string foundBadWord = word.ToLower();
                    if (!foundBadWords.ContainsKey(foundBadWord))
                    {
                        foundBadWords[foundBadWord] = new List<ProfanityLocation>();
                    }
                    foundBadWords[foundBadWord].Add(new ProfanityLocation { Position = wordIndex, Length = word.Length });
                }
                else if (useLevenshteinDistance)
                {
                    // Check if the word is similar to a bad word using Levenshtein distance
                    foreach (string badWord in badWords)
                    {
                        if (!allowWords.Contains(word) && LevenshteinDistance(word.ToLower(), badWord.ToLower()) <= 2) // Use a threshold of 2 for similarity
                        {
                            if (!foundBadWords.ContainsKey(badWord))
                            {
                                foundBadWords[badWord] = new List<ProfanityLocation>();
                            }
                            foundBadWords[badWord].Add(new ProfanityLocation { Position = wordIndex, Length = word.Length });
                        }
                    }
                }
            }

            // Check for bad words with spaces
            foreach (string badWord in badWords)
            {
                if (badWord.Contains(" ") && text.Contains(badWord))
                {
                    string foundBadWord = badWord.ToLower();
                    if (!foundBadWords.ContainsKey(foundBadWord))
                    {
                        foundBadWords[foundBadWord] = new List<ProfanityLocation>();
                    }
                    int start = text.IndexOf(badWord, StringComparison.OrdinalIgnoreCase);
                    foundBadWords[foundBadWord].Add(new ProfanityLocation { Position = start, Length = badWord.Length });
                }
            }

            return foundBadWords;
        }


        /// <summary>
        /// Finds all occurrences of bad words in the input text and censors them
        /// </summary>
        /// <param name="text">The input text to search for bad words.</param>
        /// <returns>A version of the text with all bad words censored</returns>
        public string CensorText(string text, char censorCharacter = '*')
        {
            // Find all bad words in the text
            var foundBadWords = FindProfanities(text);

            // Replace bad words with censor characters
            StringBuilder censoredText = new StringBuilder(text);
            foreach (var badWordEntry in foundBadWords)
            {
                string badWord = badWordEntry.Key;
                foreach (var indexLengthPair in badWordEntry.Value)
                {
                    int index = indexLengthPair.Position;
                    int length = indexLengthPair.Length;

                    for (int i = 0; i < length; i++)
                    {
                        censoredText[index + i] = censorCharacter;
                    }
                }
            }

            return censoredText.ToString();
        }


        
        /// <summary>
        /// Checks if there are any bad words in the input tex
        /// </summary>
        /// <param name="text">The input text to search for bad words.</param>
        /// <returns>Boolean value indicating if any bad words were found</returns>
        public bool ContainsProfanity(string text)
        {
            return FindProfanities(text).Any();
        }

        private static int LevenshteinDistance(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 0; j <= n; j++)
            {
                d[0, j] = j;
            }
            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    if (s[i - 1] == t[j - 1])
                    {
                        d[i, j] = d[i - 1, j - 1];
                    }
                    else
                    {
                        d[i, j] = Math.Min(d[i - 1, j], Math.Min(d[i, j - 1], d[i - 1, j - 1])) + 1;
                    }
                }
            }
            return d[m, n];
        }
    }
}
