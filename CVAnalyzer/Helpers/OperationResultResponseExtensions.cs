using CVAnalyzer.Models.OperationResultResponse;
using Microsoft.AspNetCore.Mvc;

namespace CVAnalyzer.Helpers
{
    public static class OperationResultResponseExtensions
    {
        public static ActionResult ToActionResult<T>(this OperationResultResponse<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Body);

            return result.Status switch
            {
                ResultStatus.InProgress => new AcceptedResult(),

                ResultStatus.BadRequest => new BadRequestObjectResult(result.Message),

                ResultStatus.NotFound => new NotFoundObjectResult(result.Message),

                ResultStatus.Unauthorized => new UnauthorizedObjectResult(result.Message),

                ResultStatus.InternalServerError => new ObjectResult(result.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },

                _ => new ObjectResult(result.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }
            };
        }
    }
}