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
            Id = dbAnalysis.Id,
            Structure = dbAnalysis.Structure ?? "Не удалось проанализировать:(",
            Technologies = dbAnalysis.Technologies ?? "Не удалось проанализировать:(",
            Relevance = dbAnalysis.Relevance ?? "Не удалось проанализировать:(",
            Another = dbAnalysis.Another ?? "Не удалось проанализировать:(",
            VacancyComparison = dbAnalysis.VacancyComparison,
            CreatedAt = dbAnalysis.CreatedAt
        };
    }
    
    public ComplexAnalysisResponse ComplexAnalysisMap(DbAnalysis dbAnalysis)
    {
        var analysis = new AnalysisResponse
        {
            Id = dbAnalysis.Id,
            Structure = dbAnalysis.Structure ?? "Не удалось проанализировать:(",
            Technologies = dbAnalysis.Technologies ?? "Не удалось проанализировать:(",
            Relevance = dbAnalysis.Relevance ?? "Не удалось проанализировать:(",
            Another = dbAnalysis.Another ?? "Не удалось проанализировать:(",
            VacancyComparison = dbAnalysis.VacancyComparison,
            CreatedAt = dbAnalysis.CreatedAt
        };
        
        if (dbAnalysis.Letter is not null)
        {
            return new ComplexAnalysisResponse
            {
                Analysis = analysis,
                Letter = new LetterResponse(dbAnalysis.Letter.Id, dbAnalysis.Letter.Text)
            };
        }
        
        return new ComplexAnalysisResponse
        {
            Analysis = analysis
        };
    }
}