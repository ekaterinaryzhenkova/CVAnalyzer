using CVAnalyzer.Business.Clients.Interfaces;
using CVAnalyzer.Business.helpers.Interfaces;
using CVAnalyzer.DbLayer.Models;
using CVAnalyzer.Mappers.Interfaces;
using CVAnalyzer.Models;
using CVAnalyzer.Models.Responses;
using CVAnalyzer.Repositories.Interfaces;
using CVAnalyzer.Repositories.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CVAnalyzer.Business.helpers
{
    public class CreateAnalysisHelper(
        IHhClient hhClient,
        IPromptService promptService,
        IAnalysisResponseMapper responseMapper,
        IAiClient aiClient,
        IAnalysisRepository analysisRepository,
        ICvRepository cvRepository,
        IMemoryCache cache,
        ILogger<ICreateAnalysisHelper> logger)
        : ICreateAnalysisHelper
    {
        private static readonly TimeSpan CacheLifeTime = TimeSpan.FromMinutes(25);
        
        public async Task ExecuteAsync()
        {
            var analysis = await analysisRepository.GetNewAnalysisAsync();

            if (analysis is null)
            {
                logger.LogInformation("No new analyses found");
                return;
            }

            await analysisRepository.UpdateAsync(analysis.Id, AnalysisStatus.Processing);
            
            var cv =  cache.TryGetValue(analysis.CvId.ToString(), out DbCV? cachedCv) && cachedCv is not null
                ? cachedCv
                : await cvRepository.GetAsync(analysis.CvId);
            
            if (cv is null)
            {
                await analysisRepository.UpdateAsync(analysis.Id, AnalysisStatus.Failed);
                return;
            }

            string vacancyText = analysis.VacancyLink is null
                ? string.Empty
                : await ParseVacancyText(analysis.VacancyLink);
            
            string prompt;
            if (string.IsNullOrEmpty(vacancyText))
            {
                string template = await promptService.GetAsync("CvAnalysis");
                prompt = string.Format(template, cv.ParsedText);
            }
            else
            {
                string template = await promptService.GetAsync("CvAndVacancyAnalysis");
                prompt = string.Format(template, cv.ParsedText, vacancyText);
            }
            
            try
            {
                var response = await aiClient.CreateAnalysisAsync(prompt);

                if (string.IsNullOrEmpty(response))
                {
                    await analysisRepository.UpdateAsync(analysis.Id, AnalysisStatus.Failed);
                    logger.LogWarning("Analysis {id} wasn't received from AI", analysis.Id);
                    return;
                }
                
                AnalysisResponse result = responseMapper.Map(response);
                
                cache.Set(analysis.Id.ToString(), result, CacheLifeTime);
                logger.LogInformation("Analysis {id} added to cache", analysis.Id);
                
                await analysisRepository.UpdateAsync(
                    analysis.Id,
                    AnalysisStatus.Done,
                    result.Structure,
                    result.Technologies,
                    result.Relevance,
                    result.Another,
                    vacancyText,
                    result.VacancyComparison);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await analysisRepository.UpdateAsync(analysis.Id, AnalysisStatus.Failed);
            }
        }

        private async Task<string> ParseVacancyText(string vacancyLink)
        {
            string[] collection = vacancyLink.Split('/');
            string vacancyId = collection[^1];
            
            int length = vacancyId.IndexOf('?');
            if (length == -1)
                length = vacancyId.Length;
            
            vacancyId = vacancyId.Substring(0, length);

            if (!int.TryParse(vacancyId, out _))
            {
                return string.Empty;
            }
            
            try
            {
                var response = await hhClient.ParseVacancyTextAsync(vacancyId);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return string.Empty;
            }
        }
    }
}