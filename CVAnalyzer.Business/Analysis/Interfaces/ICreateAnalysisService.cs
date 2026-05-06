namespace CVAnalyzer.Business.Analysis.Interfaces
{
    public interface ICreateAnalysisService
    {
        Task ExecuteAsync(Guid analysisId);
    }
}