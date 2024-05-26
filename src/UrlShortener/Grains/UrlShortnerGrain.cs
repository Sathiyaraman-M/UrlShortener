using Orleans.Runtime;
using UrlShortener.Models;

namespace UrlShortener.Grains;

[Alias(nameof(IUrlShortnerGrain))]
public interface IUrlShortnerGrain : IGrainWithStringKey
{
    [Alias(nameof(GetUrl))]
    Task<string> GetUrl();

    [Alias(nameof(SetUrl))]
    Task SetUrl(string url);
}

public sealed class UrlShortnerGrain(
    [PersistentState(StorageConstants.DEFAULT_STATE_NAME)] IPersistentState<UrlDetail> state
) : Grain, IUrlShortnerGrain
{
    public Task<string> GetUrl() => Task.FromResult(state.State.FullUrl);

    public async Task SetUrl(string url)
    {
        state.State = new UrlDetail
        {
            FullUrl = url,
            ShortenedUrlSegment = this.GetPrimaryKeyString()
        };
        await state.WriteStateAsync();
    }
}
