using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.Responses;
using System.Runtime.InteropServices.JavaScript;

namespace CVAnalyzer.Mappers
{
    public class DbAnalysisMapper : IDbAnalysisMapper
    {
        public DbAnalysis Map(string analysis)
        {
            var sections = AnalysisParser.ParseSections(analysis);

            return new DbAnalysis
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("7f679a99-6b3e-4007-8b8b-5d86bfa73902"),
                Structure = sections.GetValueOrDefault("Структура") ?? "Не удалось проанализировать:(",
                Technologies = sections.GetValueOrDefault("Техническая составляющая") ?? "Не удалось проанализировать:(",
                Relevance = sections.GetValueOrDefault("Релевантность") ?? "Не удалось проанализировать:(",
                Another = sections.GetValueOrDefault("Прочие рекомендации") ?? "Не удалось проанализировать:(",
                Date = DateOnly.FromDateTime(DateTime.Now)
            };
        }
        
        public DbAnalysis Map(AnalysisResponse analysis)
        {
            return new DbAnalysis
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("7f679a99-6b3e-4007-8b8b-5d86bfa73902"),
                Structure = analysis.Structure,
                Technologies = analysis.Technologies,
                Relevance = analysis.Relevance,
                Another = analysis.Another,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };
        }
    }
}