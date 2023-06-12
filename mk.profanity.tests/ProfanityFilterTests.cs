namespace mk.profanity.tests
{
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ProfanityFilterTests
    {
        [Test]
        public void CensorText_NoBadWords_ReturnsOriginalText()
        {
            var profanityFilter = new ProfanityFilter(options => options.SetBadWords());
            var text = "The quick brown fox jumps over the lazy dog";

            var result = profanityFilter.CensorText(text);

            Assert.AreEqual(text, result);
        }

        [Test]
        public void CensorText_OneBadWord_ReturnsCensoredText()
        {
            var profanityFilter = new ProfanityFilter(options => options.SetBadWords("fox"));
            var text = "The quick brown fox jumps over the lazy dog";

            var result = profanityFilter.CensorText(text);

            Assert.AreEqual("The quick brown *** jumps over the lazy dog", result);
        }

        [Test]
        public void CensorText_MultipleBadWords_ReturnsCensoredText()
        {
            var profanityFilter = new ProfanityFilter(options => options.SetBadWords("fox", "dog"));
            var text = "The quick brown fox jumps over the lazy dog";

            var result = profanityFilter.CensorText(text);

            Assert.AreEqual("The quick brown *** jumps over the lazy ***", result);
        }

        [Test]
        public void CensorText_AllowWords_ReturnsOriginalText()
        {
            var profanityFilter = new ProfanityFilter(options =>
                options.SetBadWords("fox", "dog")
                       .AllowWords("fox", "dog"));
            var text = "The quick brown fox jumps over the lazy dog";

            var result = profanityFilter.CensorText(text);

            Assert.AreEqual(text, result);
        }

        [Test]
        public void CensorText_UseLevenshteinDistance_ReturnsCensoredText()
        {
            var profanityFilter = new ProfanityFilter(options =>
                options.SetBadWords("fox")
                       .UseSimilarityDetection());
            var text = "The quick brown foox jumps over the lazy horse";

            var result = profanityFilter.CensorText(text);

            Assert.AreEqual("The quick brown **** jumps over the lazy horse", result);
        }



        [Test]
        public void CensorText_DefaultWordTest_ReturnsCensoredText()
        {
            var profanityFilter = new ProfanityFilter();
            var text = "you have fucking a big a s shole";

            var result = profanityFilter.CensorText(text);

            Assert.AreEqual("you have ******* a big *********", result);
        }

        [Test]
        public void ContainsProfanity()
        {
            var profanityFilter = new ProfanityFilter();
 
            Assert.IsTrue(profanityFilter.ContainsProfanity("hello fuckers"));
            Assert.IsTrue(!profanityFilter.ContainsProfanity("hello friends"));

        }

        [Test]
        public void FindProfanities()
        {
            var profanityFilter = new ProfanityFilter();

            var data = profanityFilter.FindProfanities("hello fuckers how the fuck are you?");

            Assert.IsTrue(data["fuckers"][0].Position ==  6);
            Assert.IsTrue(data["fuck"][0].Position == 22);

        }

        [Test]
        public void Configure_UseAppendBadWordsList()
        {
            var profanityFilter = new ProfanityFilter(d=>d.AppendBadWords("toast"));

            Assert.IsTrue(profanityFilter.ContainsProfanity("toast"));

        }


        [Test]
        public void Configure_AllowWords()
        {
            var profanityFilter = new ProfanityFilter(d => d.AllowWords("fuck"));

            Assert.IsTrue(!profanityFilter.ContainsProfanity("fuck"));

        }

        [Test]
        public void Configure_SimilarityDetection()
        {
            var profanityFilter = new ProfanityFilter(d => d.UseSimilarityDetection());

            Assert.IsTrue(profanityFilter.ContainsProfanity("ffck"));

        }

        [Test]
        public void Configure_WihtoutSimilarityDetection()
        {
            var profanityFilter = new ProfanityFilter();

            Assert.IsTrue(!profanityFilter.ContainsProfanity("ffck"));

        }


        [Test]
        public void Configure_SetBadWords()
        {
            var profanityFilter = new ProfanityFilter(d=>d.SetBadWords("toast","fish"));

            Assert.IsTrue(profanityFilter.ContainsProfanity("toast"));
            Assert.IsTrue(profanityFilter.ContainsProfanity("fish"));
            Assert.IsTrue(!profanityFilter.ContainsProfanity("fuck"));
        }
    }
}