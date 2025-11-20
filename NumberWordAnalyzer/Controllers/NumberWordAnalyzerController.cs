using Microsoft.AspNetCore.Mvc;
using NumberWordAnalyzer.Application;
using NumberWordAnalyzer.Services;

namespace NumberWordAnalyzer.Controllers
{
    /// <summary>
    /// Controller for analyzing concatenated words and counting occurrences of number words
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NumberWordAnalyzerController : ControllerBase
    {
        private readonly IAnalyzerService _analyzerService;

        /// <summary>
        /// Initializes a new instance of the NumberWordAnalyzerController
        /// </summary>
        /// <param name="analyzerService">The analyzer service for processing text</param>
        public NumberWordAnalyzerController(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        /// <summary>
        /// Analyzes a concatenated word (no spaces) and counts occurrences of number words (one, two, three, four, five, six, seven, eight, nine)
        /// </summary>
        /// <param name="input">The input containing the concatenated word to analyze</param>
        /// <returns>A dictionary containing each number word and its occurrence count</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/NumberWordAnalyzer
        ///     {
        ///        "inputText": "onewgftwogewgonedawdtwothreedfrfourzone"
        ///     }
        ///
        /// Sample response:
        ///
        ///     {
        ///        "one": 3,
        ///        "two": 2,
        ///        "three": 1,
        ///        "four": 1,
        ///        "five": 0,
        ///        "six": 0,
        ///        "seven": 0,
        ///        "eight": 0,
        ///        "nine": 0,
        ///     }
        ///
        /// Note: The input should be a single concatenated word without spaces.
        /// The API will find and count all occurrences of number words (one through nine) within the concatenated string.
        /// </remarks>
        /// <response code="200">Returns the analysis results with number word counts</response>
        /// <response code="400">If the input is invalid or does not meet validation requirements (e.g., empty or too long)</response>
        /// <response code="500">If an internal server error occurs during processing</response>
        [HttpPost]
        [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeNumberWordDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cleanInputText = (input.InputText ?? string.Empty).Trim();

                var analysisResult = await _analyzerService.AnalyzeText(cleanInputText);
                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occured.",
                    details = ex.Message
                });
            }
        }
    }
}
