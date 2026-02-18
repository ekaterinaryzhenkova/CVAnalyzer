using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;

namespace CVAnalyzer.Models.OperationResultResponse
{
    [JsonConverter(typeof(StringEnumConverter<,,>))]
    public enum ResultStatus
    {
        Ok,
        NotFound,
        BadRequest,
        Forbidden,
        Unauthorized,
        ExternalServerError,
        InternalServerError
    }
}