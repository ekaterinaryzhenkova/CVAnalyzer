using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Mappers.Interfaces
{
    public interface ILetterResponseMapper
    {
        LetterResponse Map(DbLetter dbLetter);
    }
}