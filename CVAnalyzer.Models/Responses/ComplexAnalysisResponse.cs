namespace CVAnalyzer.Models.Responses
{
    public record ComplexAnalysisResponse
    {
        public AnalysisResponse Analysis { get; init; }
        
        public LetterResponse Letter { get; set; }
    }
}