namespace CVAnalyzer.Models.Responses
{
    public record AnalysisResponse
    {
        public string Structure { get; init; }
        public string Technologies { get; init; }
        public string Relevance { get; init; }
        public string Another { get; init; }
    }
}