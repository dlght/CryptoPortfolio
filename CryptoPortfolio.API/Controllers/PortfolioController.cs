using CryptoPortfolio.API.Coinlore;
using CryptoPortfolio.API.DTO;
using CryptoPortfolio.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CryptoPortfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IApplicationService _applicationService;


        public PortfolioController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("RefreshPortfolio")]
        public async Task<IActionResult> RefreshPortfolio([FromQuery]long portfolioId)
        {
            var result = await this._applicationService.RefreshPortfolio(portfolioId);

            return new JsonResult(result);
        }

        [HttpPost("ImportPortfolio")]
        public async Task<ActionResult> ImportPortfolio([FromForm] FileUploadDTO fileUpload)
        {
            if (fileUpload.File == null || fileUpload.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (!fileUpload.File.ContentType.StartsWith("text/"))
            {
                return BadRequest("Only text files are allowed");
            }

            var portfolioId = await this._applicationService.ImportPortfolio(fileUpload);

            return Ok(portfolioId);
        }
    }
}