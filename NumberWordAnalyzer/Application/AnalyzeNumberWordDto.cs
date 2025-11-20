using System.ComponentModel.DataAnnotations;

namespace NumberWordAnalyzer.Application
{
    public class AnalyzeNumberWordDto
    {
        [Required(ErrorMessage = "Input text is required.")]
        [MinLength(1, ErrorMessage = "Input text cannot be empty.")]
        [MaxLength(1000, ErrorMessage = "Input text is too long.")]
        public string InputText { get; set; } = string.Empty;
    }
}
