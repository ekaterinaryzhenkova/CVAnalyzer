using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record ManualCvRequest(
        [property: Required] string FullName,
        [property: Required] string Position,
        [property: Required] string Skills,
        [property: Required] string Experience,
        [property: Required] string Education,
        [property: Required] string AboutYourself);
}