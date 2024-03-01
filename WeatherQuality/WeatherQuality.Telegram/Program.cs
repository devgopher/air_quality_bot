using BotDataSecureStorage.Settings;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using EmergencyServicesWorldwideBot.Interaction.OSM;
using EmergencyServicesWorldwideBot.Interaction.OSM.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using WeatherQuality.Infrastructure;
using WeatherQuality.Integration;
using WeatherQuality.Telegram;
using WeatherQuality.Telegram.Commands;
using WeatherQuality.Telegram.Commands.Processors;
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
    .AddScoped<IIntegration, OpenMeteoIntegration>()
    .AddHostedService<WeatherBotHostedService>()
    .AddScoped<StartCommandProcessor>()
    .AddScoped<StopCommandProcessor>()
    .AddScoped<GeoCacheExplorer>()
    .AddLocationService()
    .AddBotCommand<StartCommand, StartCommandProcessor, PassValidator<StartCommand>>()
    .AddBotCommand<StopCommand, StopCommandProcessor, PassValidator<StopCommand>>()
    .AddBotCommand<GetAirQualityCommand, GetAirQualityProcessor, PassValidator<GetAirQualityCommand>>()
    .AddBotCommand<SetLocationCommand, SetLocationProcessor, PassValidator<SetLocationCommand>>()
    .AddBotCommand<DetailsCommand, DetailsProcessor, PassValidator<DetailsCommand>>()
    .AddDbContext<WeatherQualityContext>(c => c.UseNpgsql(settings!.DbConnectionString),
        ServiceLifetime.Transient);
// builder.ApplyMigrations<WeatherQualityContext>();

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor, TelegramBot>()
    .RegisterBotCommand<StopCommand, StopCommandProcessor, TelegramBot>()
    .RegisterBotCommand<GetAirQualityCommand, GetAirQualityProcessor, TelegramBot>()
    .RegisterBotCommand<SetLocationCommand, SetLocationProcessor, TelegramBot>()
    .RegisterBotCommand<DetailsCommand, DetailsProcessor, TelegramBot>();

app.Run();