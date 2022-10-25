using Microsoft.Extensions.Options;
using Server.HostedServices;
using Telegram.Bot;
using TelegramBot.Command;
using TelegramBot.Configuration;
using TelegramBot.Handler;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllers();

builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x =>
    new TelegramBotClient(x.GetService<IOptions<TelegramOptions>>()?.Value.Token ?? string.Empty));

//Options
{
    builder.Services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.Position));
}

// Services
{
    builder.Services.AddHostedService<TelegramBotWorker>();
}

//Command
{
    builder.Services.AddTransient<ICommandFactory, CommandFactory>();
    builder.Services.AddScoped<IUpdatesHandler, UpdatesHandler>();
}

builder.Services.AddMemoryCache();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.Run();