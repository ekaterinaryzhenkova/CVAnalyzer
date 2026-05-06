using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models.Responses;

namespace CVAnalyzer.Mappers
{
    public class LetterResponseMapper : ILetterResponseMapper
    {
        public LetterResponse Map(DbLetter dbLetter)
        {
            return new LetterResponse(dbLetter.Id, dbLetter.Text);
        }
    }
}