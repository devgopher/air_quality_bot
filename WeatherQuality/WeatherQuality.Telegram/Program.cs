using BotDataSecureStorage.Settings;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using WeatherQuality.Integration;
using WeatherQuality.Telegram;
using WeatherQuality.Telegram.Commands;
using WeatherQuality.Telegram.Commands.Processors;
using WeatherQuality.Telegram.Database;
using WeatherQuality.Telegram.Extensions;
using WeatherQuality.Telegram.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(WeatherQualitySettings))
    .Get<WeatherQualitySettings>();



builder.Services
    .Configure<WeatherQualitySettings>(builder.Configuration.GetSection(nameof(WeatherQualitySettings)))
    .AddTelegramBot(builder.Configuration,
        new BotOptionsBuilder<TelegramBotSettings>()
            .Set(s => s.SecureStorageSettings = new SecureStorageSettings
            {
                ConnectionString = settings?.SecureStorageConnectionString
            })
            .Set(s => s.Name = settings?.BotName))
    .AddLogging(cfg => cfg.AddNLog())
    .AddHostedService<WeatherBotHostedService>()
    .AddScoped<IIntegration, OpenMeteoIntegration>()
    .AddScoped<StartCommandProcessor>()
    .AddScoped<StopCommandProcessor>()
    .AddBotCommand<StartCommand, StartCommandProcessor, PassValidator<StartCommand>>()
    .AddBotCommand<StopCommand, StopCommandProcessor, PassValidator<StopCommand>>()
    .AddDbContext<WeatherQualityContext>(c => c.UseNpgsql(settings!.SecureStorageConnectionString));

// builder.ApplyMigrations<WeatherQualityContext>();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor, TelegramBot>()
    .RegisterBotCommand<StopCommand, StopCommandProcessor, TelegramBot>()
    .RegisterBotCommand<GetAirQualityCommand, GetAirQualityProcessor, TelegramBot>()
    .RegisterBotCommand<SetLocationCommand, SetLocationProcessor, TelegramBot>();

app.Run();