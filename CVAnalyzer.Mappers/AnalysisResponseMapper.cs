using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.Responses;
using System.Text.RegularExpressions;

namespace CVAnalyzer.Mappers;

public class AnalysisResponseMapper : IAnalysisResponseMapper
{
    public AnalysisResponse Map(string analysis)
    {
        var sections = AnalysisParser.ParseSections(analysis);

        return new AnalysisResponse
        {
            Structure = sections.GetValueOrDefault("Структура") ?? "Не удалось проанализировать:(",
            Technologies = sections.GetValueOrDefault("Техническая составляющая") ?? "Не удалось проанализировать:(",
            Relevance = sections.GetValueOrDefault("Релевантность") ?? "Не удалось проанализировать:(",
            Another = sections.GetValueOrDefault("Прочие рекомендации") ?? "Не удалось проанализировать:(",
            VacancyComparison = sections.GetValueOrDefault("Совпадение с вакансией") ?? null
        };
    }
    
    public AnalysisResponse Map(DbAnalysis dbAnalysis)
    {
        return new AnalysisResponse
        {
            Structure = dbAnalysis.Structure ?? "Не удалось проанализировать:(",
            Technologies = dbAnalysis.Technologies ?? "Не удалось проанализировать:(",
            Relevance = dbAnalysis.Relevance ?? "Не удалось проанализировать:(",
            Another = dbAnalysis.Another ?? "Не удалось проанализировать:(",
            VacancyComparison = dbAnalysis.VacancyComparison
        };
    }
}