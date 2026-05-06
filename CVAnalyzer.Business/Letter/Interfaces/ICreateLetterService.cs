namespace CVAnalyzer.Business.Letter.Interfaces
{
    public interface ICreateLetterService
    {
        Task ExecuteAsync(Guid letterId);
    }
}