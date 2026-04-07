using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record ManualCvRequest(
        [param: Required] string FullName,
        [param: Required] string Position,
        [param: Required] string Skills,
        [param: Required] string Experience,
        [param: Required] string Education,
        [param: Required] string AboutYourself);
}