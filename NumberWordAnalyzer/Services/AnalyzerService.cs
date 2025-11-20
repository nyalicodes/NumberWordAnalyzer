using NumberWordAnalyzer.Domain;
using System.Text.RegularExpressions;

namespace NumberWordAnalyzer.Services
{
    public class AnalyzerService : IAnalyzerService
    {
        public Task<Dictionary<string, int>> AnalyzeText(string inputText)
        {
            if (inputText == null)
                throw new ArgumentNullException(nameof(inputText), "Input text cannot be null.");

            return Task.Run(() =>
            {
                var result = new Dictionary<string, int>();

                foreach (var word in NumberWords.Words)
                {
                    // Overlapping regex: (?=(word))
                    var pattern = $"(?=({Regex.Escape(word)}))";

                    var matches = Regex.Matches(inputText, pattern, RegexOptions.IgnoreCase);

                    result[word] = matches.Count;
                }

                return result;
            });
        }
    }
}
