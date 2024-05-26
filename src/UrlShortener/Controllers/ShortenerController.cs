using Microsoft.AspNetCore.Mvc;
using UrlShortener.Grains;

namespace UrlShortener.Controllers;

[ApiController]
public class ShortenerController(IGrainFactory grainFactory) : ControllerBase
{
    [HttpGet("shorten")]
    public async Task<IActionResult> Shorten(string url)
    {
        var host = GetHost(Request);

        if (string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(host, UriKind.Absolute) is false)
        {
            return BadRequest(
                $"""
                The URL query string is required and needs to be well formed.
                Consider, ${host}/shorten?url=https://www.example.com.
                """
            );
        }

        var segment = ConstructNewSegment();
        var shortenedGrain = grainFactory.GetGrain<IUrlShortnerGrain>(segment);
        await shortenedGrain.SetUrl(url);

        var shortenedUriBuilder = new UriBuilder(host) { Path = $"/go/{segment}" };
        return Ok(shortenedUriBuilder.Uri);
    }

    [HttpGet("go/{segment:required}")]
    public async Task<IActionResult> RedirectToUrl(string segment)
    {
        var shortenedGrain = grainFactory.GetGrain<IUrlShortnerGrain>(segment);

        var url = await shortenedGrain.GetUrl();

        return Redirect(url);
    }

    private static string GetHost(HttpRequest request) =>
        $"{request.Scheme}://{request.Host.Value}";

    private static string ConstructNewSegment() => Guid.NewGuid().GetHashCode().ToString("X");
}
