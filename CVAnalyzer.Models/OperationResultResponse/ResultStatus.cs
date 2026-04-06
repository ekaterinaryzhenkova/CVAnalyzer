using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CVAnalyzer.Models.OperationResultResponse
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResultStatus
    {
        Ok,
        Accepted,
        NotFound,
        BadRequest,
        Forbidden,
        Unauthorized,
        ExternalServerError,
        InternalServerError
    }
}