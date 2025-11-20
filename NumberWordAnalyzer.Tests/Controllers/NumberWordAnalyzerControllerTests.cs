using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NumberWordAnalyzer.Application;
using NumberWordAnalyzer.Controllers;
using NumberWordAnalyzer.Services;

namespace NumberWordAnalyzer.Tests.Controllers
{
    public class NumberWordAnalyzerControllerTests
    {
        private readonly Mock<IAnalyzerService> _analyzerServiceMock;
        private readonly NumberWordAnalyzerController _numberWordAnalyzerCotroller;

        public NumberWordAnalyzerControllerTests()
        {
            _analyzerServiceMock = new Mock<IAnalyzerService>();
            _numberWordAnalyzerCotroller = new NumberWordAnalyzerController(_analyzerServiceMock.Object);
        }

        [Fact]
        public async Task AnalyzeText_ValidInput_ReturnsExpectedResult()
        {
            var input = new AnalyzeNumberWordDto { InputText = "onetwothreeonenine" };

            var expectedResult = new Dictionary<string, int>
            {
                { "one", 2 },
                { "two", 1 },
                { "three", 1 },
                { "four", 0 },
                { "five", 0 },
                { "six", 0 },
                { "seven", 0 },
                { "eight", 0 },
                { "nine", 1 }
            };

            _analyzerServiceMock
                .Setup(s => s.AnalyzeText(input.InputText))
                .ReturnsAsync(expectedResult);

            var response = await _numberWordAnalyzerCotroller.Analyze(input);

            var okResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedResult, okResult.Value);

        }

        [Fact]
        public async Task AnalyzeText_NullModel_ReturnsBadRequest()
        {
            _numberWordAnalyzerCotroller.ModelState.AddModelError("InputText", "The InputText field is required.");

            var response = await _numberWordAnalyzerCotroller.Analyze(null!);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Analyze_ServiceThrowsException_Returns500()
        {
            var input = new AnalyzeNumberWordDto { InputText = "test" };

            _analyzerServiceMock
                .Setup(s => s.AnalyzeText(input.InputText))
                .ThrowsAsync(new Exception("Test error"));

            var result = await _numberWordAnalyzerCotroller.Analyze(input);

            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
