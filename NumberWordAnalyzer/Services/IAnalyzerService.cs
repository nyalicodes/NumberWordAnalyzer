namespace NumberWordAnalyzer.Services
{
    public interface IAnalyzerService
    {
        Task<Dictionary<string, int>> AnalyzeText(string inputText);
    }
}
