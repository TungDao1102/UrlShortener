using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Extensions;
using UrlShortener.ApplicationCore.DTOs.Request;
using UrlShortener.ApplicationCore.Interfaces.Services;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController(IUrlService urlService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateShortenUrlAsync([FromBody] CreateUrlRequest request, CancellationToken cancellationToken)
        {
            var email = HttpContext.User.GetUserEmail();
            var requestWithUser = request with
            {
                CreatedBy = email
            };
            var result = await urlService.CreateShortenUrlAsync(requestWithUser, cancellationToken);

            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Created($"/api/create/{result.Value!.ShortUrl}", result.Value);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllShortenUrlAsync()
        {
            var request = new GetListUrlRequest(HttpContext.User.GetUserEmail());
            return Ok(await urlService.GetListShortenUrlAsync(request, CancellationToken.None));
        }
    }
}
