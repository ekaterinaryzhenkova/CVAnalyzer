using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Mappers.Interfaces
{
    public interface IAnalysisResponseMapper
    {
        AnalysisResponse Map(string analysis);

        AnalysisResponse Map(DbAnalysis dbAnalysis);
    }
}