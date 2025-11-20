using NumberWordAnalyzer.Services;

namespace NumberWordAnalyzer.Tests.Services
{
    public class AnalyzerServiceTests
    {
        private readonly AnalyzerService _analyzerService;

        public AnalyzerServiceTests()
        {
            _analyzerService = new AnalyzerService();
        }

        [Theory]
        [InlineData("onetwothreeone", 2, 1, 1, 0, 0, 0, 0, 0, 0)]
        [InlineData("fivefivefive", 0, 0, 0, 0, 3, 0, 0, 0, 0)]
        [InlineData("twoonethree", 1, 1, 1, 0, 0, 0, 0, 0, 0)]
        public void AnalyzeText_ShouldReturnCorrectCounts(string input, params int[] expectedCounts)
        {
            var expected = new Dictionary<string, int>
            {
                { "one", expectedCounts[0] },
                { "two", expectedCounts[1] },
                { "three", expectedCounts[2] },
                { "four", expectedCounts[3] },
                { "five", expectedCounts[4] },
                { "six", expectedCounts[5] },
                { "seven", expectedCounts[6] },
                { "eight", expectedCounts[7] },
                { "nine", expectedCounts[8] }
            };

            var result = _analyzerService.AnalyzeText(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AnalyzeText_EmptyString_ShouldReturnAllZeroCounts()
        {
            var result = _analyzerService.AnalyzeText("abcdefghijklmnopqrstuvwxyz");

            Assert.All(result.Values, count => Assert.Equal(0, count));
        }

        [Fact]
        public void AnalyzeText_NullInput_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _analyzerService.AnalyzeText(null));
        }
    }
}
