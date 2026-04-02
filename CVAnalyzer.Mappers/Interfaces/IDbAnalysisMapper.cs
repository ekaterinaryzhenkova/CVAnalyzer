using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Mappers.Interfaces
{
    public interface IDbAnalysisMapper
    {
        DbAnalysis Map(string analysis);

        DbAnalysis Map(AnalysisResponse analysis, Guid cvId);
    }
}