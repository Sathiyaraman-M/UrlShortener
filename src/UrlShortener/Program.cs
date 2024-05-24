using UrlShortener;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage(StorageConstants.DEFAULT_STORAGE_NAME);
});

app.MapGet("/", () => "URL Shortner using Microsoft Orleans");

app.Run();
