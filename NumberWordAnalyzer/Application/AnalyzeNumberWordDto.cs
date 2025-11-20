using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NumberWordAnalyzer.Application
{
    /// <summary>
    /// Data transfer object for number word analysis requests
    /// </summary>
    public class AnalyzeNumberWordDto
    {
        /// <summary>
        /// A single concatenated word (no spaces) to analyze for counting occurrences of number words (one through nine)
        /// </summary>
        /// <example>onewgftwogewgonedawdtwothreedfrfourzone</example>
        [Required(ErrorMessage = "Input text is required.")]
        [MinLength(1, ErrorMessage = "Input text cannot be empty.")]
        [MaxLength(1000, ErrorMessage = "Input text is too long.")]
        [Description("A concatenated word without spaces. Must be between 1 and 1000 characters.")]
        public string InputText { get; set; } = string.Empty;
    }
}
