using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public struct ManualCvRequest
    {
        [Required]
        public string FullName { get; init; }
        
        [Required]
        public string Position { get; init; }
    
        [Required]
        public string Skills { get; init; }
    
        [Required]
        public string Experience { get; init; }
        
        [Required]
        public string Education { get; init; }
        
        [Required]
        public string AboutYourself { get; init; }
    }
}