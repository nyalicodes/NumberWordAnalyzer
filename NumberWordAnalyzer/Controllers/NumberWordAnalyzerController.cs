using Microsoft.AspNetCore.Mvc;
using NumberWordAnalyzer.Application;
using NumberWordAnalyzer.Services;

namespace NumberWordAnalyzer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberWordAnalyzerController : ControllerBase
    {
        private readonly IAnalyzerService _analyzerService;

        public NumberWordAnalyzerController(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpPost]
        public IActionResult Analyze([FromBody] AnalyzeNumberWordDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cleanInputText = (input.InputText ?? string.Empty).Trim();

                var analysisResult = _analyzerService.AnalyzeText(cleanInputText);
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
