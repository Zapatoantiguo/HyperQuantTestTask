using HyperQuantTestTask.BitfinexLib;
using HyperQuantTestTask.WebApi;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<BitfinexRestApiClient>();
builder.Services.AddSingleton<BitfinexWsClient>();
builder.Services.AddSingleton<ITestConnector, TestConnector>();
builder.Services.AddSingleton<WebsocketDataLogger>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
