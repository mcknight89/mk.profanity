[![Unit Tests](https://github.com/mcknight89/mk.profanity/actions/workflows/tests.yml/badge.svg)](https://github.com/mcknight89/mk.profanity/actions/workflows/tests.yml)
# mk.profanity 
## About
mk.profanity is a powerful C# library designed to detect and censor profanities within text strings. This library provides several features, including text censorship, locating the position of bad words, setting custom bad word lists, appending words to the bad word list, and whitelisting words contained within the bad word list.

## Installation

You can easily install mk.profanity via NuGet. Follow these steps:

1. Open the NuGet Package Manager Console in Visual Studio.

2. Run the following command to install the mk.profanity package:

```shell
Install-Package mk.profanity
```

## Usage
To start using mk.profanity in your code, follow the examples below:

```csharp
using mk.profanity;

// Create an instance of the ProfanityFilter
ProfanityFilter profanityFilter = new ProfanityFilter();

// Set custom options (optional)
profanityFilter = new ProfanityFilter(options =>
{
    options.SetBadWords("bad", "inappropriate", "words");
    options.AppendBadWords("unacceptable");
    options.AllowWords("good", "acceptable");
});

// Detect and censor profanities
string inputText = "The quick brown fox jumps over the lazy dog";
string censoredText = profanityFilter.CensorText(inputText);
```
The resulting censoredText will be: "The quick brown *** jumps over the lazy dog" if the word "fox" is considered a bad word.

You can also locate the positions of bad words in the text using the FindProfanities method:

```csharp
string inputText = "I don't want to see any bad words in this paragraph, like jerk, moron, and imbecile.";
Dictionary<string, List<ProfanityFilter.ProfanityLocation>> foundProfanities = profanityFilter.FindProfanities(inputText);
The foundProfanities dictionary will contain bad words as keys and lists of ProfanityLocation objects as values, representing the positions and lengths of the bad words in the input text.
```

## Options
mk.profanity provides options to customize its behavior. You can pass a configuration delegate to the ProfanityFilter constructor to set these options:

```csharp
ProfanityFilter profanityFilter = new ProfanityFilter(options =>
{
    options.SetBadWords("bad", "inappropriate", "words");
    options.AppendBadWords("unacceptable");
    options.AllowWords("good", "acceptable");
    options.UseSimilarityDetection(); // Enable similarity detection
});
```
The available options are:

- SetBadWords: Replace the standard bad words (profanities) list with a custom list.
- AppendBadWords: Append additional bad words (profanities) to the existing list.
- AllowWords: Add a list of words that should be allowed even if they are in the profanity list.
- UseSimilarityDetection: Enable the use of the Levenshtein Distance Algorithm to detect similar words to bad words.

## Contributing
Contributions to mk.profanity are welcome! If you find a bug or have a suggestion for improvement, please feel free to open an issue or submit a pull request on the GitHub repository.

## License
This project is licensed under the MIT License.
