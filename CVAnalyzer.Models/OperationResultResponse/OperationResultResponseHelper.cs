using System.Net;

namespace CVAnalyzer.Models.OperationResultResponse
{
    public static class OperationResultResponseHelper
    {
        public static async Task<OperationResultResponse<T>>
            HttpToOperationResultAsync<T>(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Unauthorized =>
                    new OperationResultResponse<T>(
                        "Unauthorized. Token may be expired.",
                        ResultStatus.Unauthorized),
                    
                HttpStatusCode.BadRequest =>
                    new OperationResultResponse<T>(
                        "Bad request to external service.",
                        ResultStatus.BadRequest),

                HttpStatusCode.Forbidden =>
                    new OperationResultResponse<T>(
                        "Access denied.",
                        ResultStatus.Forbidden),

                HttpStatusCode.NotFound =>
                    new OperationResultResponse<T>(
                        "Object not found.",
                        ResultStatus.NotFound),

                _ =>
                    new OperationResultResponse<T>(
                        "External service error.",
                        ResultStatus.ExternalServerError)
            };
        }
    }
}