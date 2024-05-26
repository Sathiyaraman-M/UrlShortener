using UrlShortener;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(
    StorageConstants.DEFAULT_CONNECTION_NAME
);
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddAdoNetGrainStorageAsDefault(options =>
    {
        options.Invariant = StorageConstants.DEFAULT_STORAGE_INVARIANT;
        options.ConnectionString = connectionString;
    });
});

builder.Services.AddHealthChecks().AddSqlServer(connectionString);

builder.Services.AddControllers();

using var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/healthz");
app.MapGet("/", () => "URL Shortner using Microsoft Orleans");

app.Run();
