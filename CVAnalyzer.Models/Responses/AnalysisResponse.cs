namespace CVAnalyzer.Models.Responses
{
    public record AnalysisResponse
    {
        public Guid Id { get; init; }
        public string Structure { get; init; }
        public string Technologies { get; init; }
        public string Relevance { get; init; }
        public string Another { get; init; }
        public string? VacancyComparison { get; init; }
        
        public DateTime CreatedAt { get; init; }
    }
}