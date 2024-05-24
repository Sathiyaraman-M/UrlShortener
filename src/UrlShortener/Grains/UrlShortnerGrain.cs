using Orleans.Runtime;
using UrlShortener.Models;

namespace UrlShortener.Grains;

[Alias("IUrlShortnerGrain")]
public interface IUrlShortnerGrain : IGrainWithStringKey
{
    [Alias("GetUrl")]
    Task<string> GetUrl();

    [Alias("SetUrl")]
    Task SetUrl(string url);
}

public sealed class UrlShortnerGrain([PersistentState(stateName: "url", storageName: StorageConstants.DEFAULT_STORAGE_NAME)] IPersistentState<UrlDetail> state) : Grain, IUrlShortnerGrain
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
