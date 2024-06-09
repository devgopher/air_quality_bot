using Botticelli.Framework.SendOptions;
using Botticelli.Framework.Telegram;
using Botticelli.Interfaces;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Infrastructure;
using WeatherQuality.Infrastructure.Models;
using WeatherQuality.Integration.Interfaces;
using WeatherQuality.Telegram.Commands.ReplyOptions;
using WeatherQuality.Telegram.Settings;

namespace WeatherQuality.Telegram.Services;

public class AqiDataProcessor : IAqiDataProcessor, IDetailsDataProcessor
{
    private readonly IOptionsSnapshot<WeatherQualitySettings> _settings;
    private readonly IAirQualityIntegration _airQualityIntegration;
    private readonly GeoCacheExplorer _geoCacheExplorer;
    private readonly IBot _bot;
    private readonly IGetLocationService _getLocationService;

    public AqiDataProcessor(IOptionsSnapshot<WeatherQualitySettings> settings,
        IAirQualityIntegration airQualityIntegration, GeoCacheExplorer geoCacheExplorer,
        IBot<TelegramBot> bot,
        IGetLocationService getLocationService)
    {
        _settings = settings;
        _airQualityIntegration = airQualityIntegration;
        _geoCacheExplorer = geoCacheExplorer;
        _bot = bot;
        _getLocationService = getLocationService;
    }
    
    public async Task DetailsProcess(Message message, CancellationToken token)
    {
        var location = _getLocationService.GetLocation(message);
        var address = await _getLocationService.GetAddress(location);
        var elements = typeof(Current).GetJsonPropertyNames().Where(x => x != "time" && x != "interval").ToList();
        var systemResponse = await ProcessCache(token, elements, location);
        var respMessage = CreateResponseMessage(message, "\ud83c\udf0f Main air quality indicators",
            $"\n\n\ud83d\uddfa Address: {address}" +
            $"\n\n{_settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "carbon_monoxide"
                                                                  && c.LowBorder <= systemResponse.Current?.CarbonMonoxide
                                                                  && c.HiBorder > systemResponse.Current?.CarbonMonoxide)
                ?.Text} Carbon monoxide (CO):  {systemResponse.Current?.CarbonMonoxide} μg/m³ \n" +
            $"{_settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "nitrogen_dioxide"
                                                              && c.LowBorder <= systemResponse.Current?.NitrogenDioxide
                                                              && c.HiBorder > systemResponse.Current?.NitrogenDioxide)
                ?.Text} Nitrogen dioxide (NO\u2082): {systemResponse.Current?.NitrogenDioxide} μg/m³ \n" +
            $"{_settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "sulphur_dioxide"
                                                              && c.LowBorder <= systemResponse.Current?.SulphurDioxide
                                                              && c.HiBorder > systemResponse.Current?.SulphurDioxide)
                ?.Text} Sulphur dioxide: {systemResponse.Current?.SulphurDioxide} μg/m³ \n" +
            $"{_settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "pm10"
                                                              && c.LowBorder <= systemResponse.Current?.Pm10
                                                              && c.HiBorder > systemResponse.Current?.Pm10)
                ?.Text} Dust (PM10): {systemResponse.Current?.Pm10} μg/m³ \n" +
            $"{_settings.Value?.Criteria?.SingleOrDefault(c => c.Name == "pm25"
                                                              && c.LowBorder <= systemResponse.Current?.Pm25
                                                              && c.HiBorder > systemResponse.Current?.Pm25)
                ?.Text} Dust (PM25): {systemResponse.Current?.Pm25} μg/m³");

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, Replies.GeneralReplyOptions, token);
    }

    public async Task AqiProcess(Message message, CancellationToken token)
    {
        var location = _getLocationService.GetLocation(message);
        var elements = new List<string>()
        {
            "european_aqi"
        };

        var systemResponse = await ProcessCache(token, elements, location);
        var aqi = systemResponse.Current?.EuropeanAqi;
        var decision =
            _settings.Value.Criteria?.SingleOrDefault(c =>
                aqi >= c.LowBorder && aqi < c.HiBorder && c.Name == "european_aqi");
        if (decision == default)
            throw new InvalidDataException($"Can't get an appropriate decision for aqi {aqi}! " +
                                           $"Please, check out your configuration ('Criteria' section)!");

        var generatedImage = GenerateImage(systemResponse, decision);

        var address =
            await _getLocationService.GetAddress(location);

        var respMessage = CreateResponseMessage(message,
            decision.Text,
            !string.IsNullOrWhiteSpace(systemResponse.Error)
                ? systemResponse.Error
                : address, generatedImage);

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, Replies.GeneralReplyOptions, token);
    }

    private async Task<Response> ProcessCache(CancellationToken token, List<string> elements,
        UserLocationModel? location)
    {
        // Get available info from cache
        var cachedItemsQuery = elements.Select(async e => await _geoCacheExplorer.UpsertToCacheAsync(e,
            location.Latitude,
            location.Longitude,
            _settings.Value?.GeoCachingRadius ?? 5.0,
            _settings.Value?.CachingPeriod ?? 2.0,
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

        var integrationResponse = await _airQualityIntegration.GetAirQualityAsync(request);
        var current = integrationResponse.Current;
        if (current != default)
            cachedResponse.Current.MergeByDefaultValues(current);

        // caching non-cached data
        foreach (var element in integrationResponse.Current
                     .GetJsonProperties(true)
                     .Where(element => elements.Contains(element.Key)))
        {
            var geoCacheModel = await _geoCacheExplorer.UpsertToCacheAsync(element.Key,
                location.Latitude,
                location.Longitude,
                _settings.Value?.GeoCachingRadius ?? 5.0,
                _settings.Value?.CachingPeriod ?? 2.0,
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

    private static Message? CreateResponseMessage(Message message, string subj, string? body, byte[]? image = null)
    {
        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = subj,
            Body = body
        };

        if (image != null)
            respMessage.Attachments = new List<BaseAttachment>()
            {
                new BinaryBaseAttachment(Guid.NewGuid().ToString(),
                    "air",
                    MediaType.Image,
                    string.Empty,
                    image)
            };

        return respMessage;
    }

    private static byte[] GenerateImage(Response response, MetricCriteria criteria)
    {
        var image = ImageUtils.PlaceText(criteria.ImagePath,
            $"AQI: {response.Current?.EuropeanAqi}", 200f,
            Color.Red, 200, 50);

        return criteria.Recommendations.Aggregate(image, (current, recommendation)
            => ImageUtils.PlaceText(current, recommendation,
                100f,
                Color.Parse(criteria.Color),
                200,
                250 + criteria.Recommendations.IndexOf(recommendation) * 100));
    }
}