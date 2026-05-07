using CurrencyDeltaApi.Clients;
using CurrencyDeltaApi.Middleware;
using CurrencyDeltaApi.Services;
using CurrencyDeltaApi.Strategy;
using CurrencyDeltaApi.Validation;

// Konfigurerar ASP.NET Core-applikationen
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Registrera tjänster i dependency injection-containern
builder.Services.AddSingleton<IRequestValidator, RequestValidator>();
// Konfigurera HTTP-klient för Riksbankens API
builder.Services.AddHttpClient<IRiksbankApiClient, RiksbankApiClient>(client =>
{
    string baseUrl = builder.Configuration["RiksbankApi:BaseUrl"] ?? "https://api.riksbank.se/swea/v1";
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<IRateStrategyFactory, RateStrategyFactory>();
builder.Services.AddScoped<ICurrencyDeltaService, CurrencyDeltaService>();

var app = builder.Build();

// Konfigurera middleware-pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
