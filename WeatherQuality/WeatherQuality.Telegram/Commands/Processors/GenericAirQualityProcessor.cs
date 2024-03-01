using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using EmergencyServicesWorldwideBot.Interaction.OSM;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Infrastructure;
using WeatherQuality.Infrastructure.Models;
using WeatherQuality.Integration;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram.Commands.Processors;

public abstract class GenericAirQualityProcessor<T> : CommandProcessor<T> where T : class, ICommand
{
    protected readonly IOptionsSnapshot<WeatherQualitySettings> Settings;
    private readonly IIntegration _integration;
    private readonly GeoCacheExplorer _geoCacheExplorer;
    private readonly IServiceProvider _sp;
    protected readonly ILocationService LocationService;
    protected readonly SendOptionsBuilder<ReplyMarkupBase> Options;

    protected GenericAirQualityProcessor(ILogger logger,
        IOptionsSnapshot<WeatherQualitySettings> settings,
        ICommandValidator<T> validator, MetricsProcessor metricsProcessor, IIntegration integration,
        GeoCacheExplorer geoCacheExplorer, IServiceProvider sp, ILocationService locationService) : base(logger,
        validator, metricsProcessor)
    {
        Settings = settings;
        _integration = integration;
        _geoCacheExplorer = geoCacheExplorer;
        _sp = sp;
        LocationService = locationService;
        Options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("/Details")
                {
                    RequestLocation = false
                },
                new KeyboardButton("/GetAirQuality")
                {
                    RequestLocation = false
                },
                new KeyboardButton("/SetLocation")
                {
                    RequestLocation = true
                }
            }
        })
        {
            ResizeKeyboard = true,
            IsPersistent = true
        });
    }

    protected async Task<Response> ProcessCache(CancellationToken token, List<string> elements,
        UserLocationModel? location)
    {
        // Get available info from cache
        var cachedItemsQuery = elements.Select(async e => await _geoCacheExplorer.UpsertToCacheAsync(e,
            location.Latitude,
            location.Longitude,
            Settings.Value?.GeoCachingRadius ?? 5.0,
            Settings.Value?.CachingPeriod ?? 2.0,
            null!,
            token));

        var cachedItems = await Task.WhenAll(cachedItemsQuery.ToList());

        // Requiring needed values from source
        var nullCacheValues = cachedItems
            .Where(c => c is { SerializedValue: null })
            .ToList();

        var actualCacheValues = cachedItems
            .Except(nullCacheValues)
            .Where(c => c != null && c is not { SerializedValue: null })
            .ToList();

        var cachedResponse = new Response
        {
            IsSuccess = true,
            Latitude = (double)location.Latitude,
            Longitude = (double)location.Longitude,
            Hourly = new Hourly(),
            Current = new Current()
        };

        foreach (var element in actualCacheValues)
            cachedResponse.Current.SetJsonDeserializedProperty(element.ElementName, element.SerializedValue);

        var elementsToRequest = nullCacheValues.Select(v => v?.ElementName).ToList();

        var request = new Request
        {
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Current = !elementsToRequest.Any() ? elements : elementsToRequest,
            Hourly = string.Empty,
            Timezone = string.Empty
        };

        var integrationResponse = await _integration.GetAirQualityAsync(request);
        var current = integrationResponse.Current;
        if (current != default)
            cachedResponse.Current.MergeByDefaultValues(current);

        // caching non-cached data
        foreach (var element in integrationResponse.Current.GetJsonProperties(true))
        {
            if (!elements.Contains(element.Key))
                continue;

            var geoCacheModel = await _geoCacheExplorer.UpsertToCacheAsync(element.Key,
                location.Latitude,
                location.Longitude,
                Settings.Value?.GeoCachingRadius ?? 5.0,
                Settings.Value?.CachingPeriod ?? 2.0,
                element.Value,
                token);

            if (geoCacheModel == default)
                continue;

            // mixing up cached and non-cached data
            cachedResponse.Current.SetJsonDeserializedProperty(geoCacheModel.ElementName,
                geoCacheModel.SerializedValue);
        }

        return cachedResponse;
    }

    protected static Message? CreateResponseMessage(Message message, string subj, string body, byte[]? image = null)
    {
        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = subj,
            Body = body
        };

        if (image != null)
            respMessage.Attachments = new List<IAttachment>()
            {
                new BinaryAttachment(Guid.NewGuid().ToString(),
                    "air",
                    MediaType.Image,
                    string.Empty,
                    image)
            };

        return respMessage;
    }

    protected UserLocationModel? GetLocation(Message message)
    {
        var context = _sp.GetService<WeatherQualityContext>();

        return context.UserLocationModels.FirstOrDefault(um => message.ChatIds.Contains(um.ChatId));
    }
}