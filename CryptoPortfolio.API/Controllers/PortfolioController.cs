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
        private readonly ApplicationService _applicationService;


        public PortfolioController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("GetPortfolio")]
        public async Task<IActionResult> GetUpdatedPortfolio([FromQuery]long portfolioId)
        {
            var result = await this._applicationService.GetUpdatePortfolio(portfolioId);

            return new JsonResult(result);
        }

        [HttpPost("Upload")]
        public async Task<ActionResult> Upload([FromForm] FileUploadDTO fileUpload)
        {
            if (fileUpload.File == null || fileUpload.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (!fileUpload.File.ContentType.StartsWith("text/")) // Check for text/*
            {
                return BadRequest("Only text files are allowed");
            }

            var portfolioId = await this._applicationService.CreatePortfolio(fileUpload);

            return Ok(portfolioId);
        }
    }
}