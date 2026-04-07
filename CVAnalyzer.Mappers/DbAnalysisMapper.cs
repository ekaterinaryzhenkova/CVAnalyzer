using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Mappers
{
    public class DbAnalysisMapper : IDbAnalysisMapper
    {
        public DbAnalysis Map(string analysis, Guid cvId)
        {
            var sections = AnalysisParser.ParseSections(analysis);

            return new DbAnalysis
            {
                Id = Guid.NewGuid(),
                CvId = cvId,
                Structure = sections.GetValueOrDefault("Структура") ?? "Не удалось проанализировать:(",
                Technologies = sections.GetValueOrDefault("Техническая составляющая") ?? "Не удалось проанализировать:(",
                Relevance = sections.GetValueOrDefault("Релевантность") ?? "Не удалось проанализировать:(",
                Another = sections.GetValueOrDefault("Прочие рекомендации") ?? "Не удалось проанализировать:(",
                VacancyComparison = sections.GetValueOrDefault("Совпадение с вакансией"),
                CreatedAt = DateTime.UtcNow
            };
        }
        
        public DbAnalysis Map(AnalysisResponse analysis, Guid cvId)
        {
            return new DbAnalysis
            {
                Id = Guid.NewGuid(),
                CvId = cvId,
                Structure = analysis.Structure,
                Technologies = analysis.Technologies,
                Relevance = analysis.Relevance,
                Another = analysis.Another,
                VacancyComparison = analysis.VacancyComparison,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}