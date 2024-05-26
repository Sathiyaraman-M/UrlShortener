using UrlShortener;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddAdoNetGrainStorageAsDefault(options =>
    {
        options.Invariant = StorageConstants.DEFAULT_STORAGE_INVARIANT;
        options.ConnectionString = builder.Configuration.GetConnectionString(StorageConstants.DEFAULT_CONNECTION_NAME);
    });
});

using var app = builder.Build();

app.MapGet("/", () => "URL Shortner using Microsoft Orleans");

app.Run();
