namespace NumberWordAnalyzer.Services
{
    public interface IAnalyzerService
    {
        Dictionary<string, int> AnalyzeText(string inputText);
    }
}
