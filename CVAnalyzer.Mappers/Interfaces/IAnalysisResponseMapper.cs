using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Mappers.Interfaces
{
    public interface IAnalysisResponseMapper
    {
        AnalysisResponse Map(string analysis);
    }
}