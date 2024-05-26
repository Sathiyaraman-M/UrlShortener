var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddAdoNetGrainStorageAsDefault(options =>
    {
        options.Invariant = "System.Data.SqlClient";
        options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    });
});

using var app = builder.Build();

app.MapGet("/", () => "URL Shortner using Microsoft Orleans");

app.Run();
