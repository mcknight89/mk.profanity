using System;
using System.Collections.Generic;
using System.Linq;

namespace mk.profanity
{
    public class ProfanityFilterOptions
    {
        internal List<string>? UseAllowWordList { get; private set; } = null;
        internal List<string>? UseBadWordsList { get; private set; } = null;
        internal List<string>? UseAppendBadWordsList { get; private set; } = null;
        internal bool UseLevenshtein { get; private set; } = false;

        /// <summary>
        /// Add a list of words that should be allowed even if they are in the profanity list
        /// </summary>
        public ProfanityFilterOptions AllowWords(params string[] words)
        {
            UseAllowWordList = words.ToList();
            return this;
        }

        /// <summary>
        /// Replace the standard bad words (profanities) list with a custom list
        /// </summary>
        public ProfanityFilterOptions SetBadWords(params string[] words)
        {
            UseBadWordsList = words.ToList();
            return this;
        }

        /// <summary>
        /// Append additional bad words (profanities) to the exisitng list
        /// </summary>
        public ProfanityFilterOptions AppendBadWords(params string[] words)
        {
            UseAppendBadWordsList = words.ToList();
            return this;
        }

        /// <summary>
        /// Use the Levenshtein Distance Algorithm to detect simualar words to our bad words like misspelled words etc  
        /// </summary>
        //public ProfanityFilterOptions UseLevenshteinDistance()
        //{
        //    UseLevenshtein = true;
        //    return this;
        //}

        /// <summary>
        /// Use the Levenshtein Distance Algorithm to detect simualar words to our bad words like misspelled words etc  
        /// </summary>
        public ProfanityFilterOptions UseSimilarityDetection()
        {
            UseLevenshtein = true;
            return this;
        }
    }
}
