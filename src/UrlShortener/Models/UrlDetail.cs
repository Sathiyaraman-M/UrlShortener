namespace UrlShortener.Models;

[GenerateSerializer]
[Alias(nameof(UrlDetail))]
public record class UrlDetail
{
    [Id(0)]
    public string FullUrl { get; set; } = "";

    [Id(1)]
    public string ShortenedUrlSegment { get; set; } = "";
}