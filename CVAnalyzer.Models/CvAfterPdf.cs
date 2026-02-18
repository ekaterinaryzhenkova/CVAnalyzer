using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models;

public class CvAfterPdf
{
    [Required]
    public string ParsedText { get; set; }
    
    public string Education { get; set; }
    
    public string Skills { get; set; }
    
    public string Experience { get; set; }
}