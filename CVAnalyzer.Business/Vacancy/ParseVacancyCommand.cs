using CVAnalyzer.Business.Interfaces;
using CVAnalyzer.Business.Vacancy.Interfaces;
using CVAnalyzer.Models.OperationResultResponse;
using CVAnalyzer.Models.Requests;
using System.Net;

namespace CVAnalyzer.Business.Vacancy
{
    public class ParseVacancyCommand(IHhClient hhClient) : IParseVacancyCommand
    {
        public async Task<OperationResultResponse<string>> ExecuteAsync(VacancyRequest vacancy)
        {
            //https://spb.hh.ru/vacancy/130452842?hhtmFromLabel=suitable_vacancies_sidebar&hhtmFrom=vacancy
            string[] collection = vacancy.link.Split('/');
            string vacancyId = collection[^1];
            
            int length = vacancyId.IndexOf('?');
            if (length == -1)
                length = vacancyId.Length;
            
            vacancyId = vacancyId.Substring(0, length);

            if (!int.TryParse(vacancyId, out _))
            {
                return new OperationResultResponse<string>(
                    "The vacancy link is incorrect.",
                    ResultStatus.BadRequest);
            }

            try
            {
                var response = await hhClient.ParseVacancyAsync(vacancyId);

                if (response.StatusCode is HttpStatusCode.OK)
                {
                    return new OperationResultResponse<string>(response.Content);
                }

                return await OperationResultResponseHelper.HttpToOperationResultAsync<string>(response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                return new OperationResultResponse<string>(
                    ex.Message,
                    ResultStatus.ExternalServerError);
            }
            catch (Exception)
            {
                return new OperationResultResponse<string>(
                    "Unexpected error.",
                    ResultStatus.InternalServerError);
            }
        }
    }
}