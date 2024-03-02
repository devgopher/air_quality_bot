using BotDataSecureStorage.Settings;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Extensions;
using Botticelli.Framework.Options;
using Botticelli.Framework.Telegram;
using Botticelli.Framework.Telegram.Extensions;
using Botticelli.Framework.Telegram.Options;
using Botticelli.Scheduler;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using WeatherQuality.Infrastructure;
using WeatherQuality.Integration;
using WeatherQuality.Integration.Extensions;
using WeatherQuality.Integration.Interfaces;
using WeatherQuality.Integration.Settings;
using WeatherQuality.Telegram;
using WeatherQuality.Telegram.Commands;
using WeatherQuality.Telegram.Commands.Processors;
using WeatherQuality.Telegram.Jobs;
using WeatherQuality.Telegram.Settings;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration
    .GetSection(nameof(WeatherQualitySettings))
    .Get<WeatherQualitySettings>();

builder.Services
    .Configure<WeatherQualitySettings>(builder.Configuration.GetSection(nameof(WeatherQualitySettings)))
    .Configure<LocationCacheSettings>(builder.Configuration.GetSection(nameof(LocationCacheSettings)))
    .AddTelegramBot(builder.Configuration,
        new BotOptionsBuilder<TelegramBotSettings>()
            .Set(s => s.SecureStorageSettings = new SecureStorageSettings
            {
                ConnectionString = settings?.SecureStorageConnectionString
            })
            .Set(s => s.Name = settings?.BotName))
    .AddLogging(cfg => cfg.AddNLog())
    .AddScoped<IAirQualityIntegration, OpenMeteoAirQualityIntegration>()
    .AddHostedService<WeatherBotHostedService>()
    .AddScoped<StartCommandProcessor>()
    .AddScoped<StopCommandProcessor>()
    .AddScoped<GeoCacheExplorer>()
    .AddScoped<ClearCacheJob>()
    .AddCachedLocationService()
    .AddBotCommand<StartCommand, StartCommandProcessor, PassValidator<StartCommand>>()
    .AddBotCommand<StopCommand, StopCommandProcessor, PassValidator<StopCommand>>()
    .AddBotCommand<GetAirQualityCommand, GetAirQualityProcessor, PassValidator<GetAirQualityCommand>>()
    .AddBotCommand<SetLocationCommand, SetLocationProcessor, PassValidator<SetLocationCommand>>()
    .AddBotCommand<DetailsCommand, DetailsProcessor, PassValidator<DetailsCommand>>()
    .AddHangfire(cfg => cfg
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseActivator(new HangfireActivator(builder.Services))
        .UseRecommendedSerializerSettings()
        .UseMemoryStorage()
        .UseNLogLogProvider())
    .AddHangfireServer(opt => opt.CancellationCheckInterval = TimeSpan.FromSeconds(30))
    .AddDbContext<WeatherQualityContext>(c => c.UseNpgsql(settings!.DbConnectionString),
        ServiceLifetime.Transient);

var app = builder.Build();
app.Services.RegisterBotCommand<StartCommand, StartCommandProcessor, TelegramBot>()
    .RegisterBotCommand<StopCommand, StopCommandProcessor, TelegramBot>()
    .RegisterBotCommand<GetAirQualityCommand, GetAirQualityProcessor, TelegramBot>()
    .RegisterBotCommand<SetLocationCommand, SetLocationProcessor, TelegramBot>()
    .RegisterBotCommand<DetailsCommand, DetailsProcessor, TelegramBot>();

app.Run();
