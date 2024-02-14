using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using SixLabors.ImageSharp;
using WeatherQuality.Domain.Request;
using WeatherQuality.Domain.Response;
using WeatherQuality.Integration;
using WeatherQuality.Telegram.Database;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : CommandProcessor<GetAirQualityCommand>
{
    private readonly IIntegration _integration;
    private readonly WeatherQualityContext _context;

    public GetAirQualityProcessor(ILogger<GetAirQualityProcessor> logger, ICommandValidator<GetAirQualityCommand> validator,
        MetricsProcessor metricsProcessor, IIntegration integration, WeatherQualityContext context) : base(logger, validator, metricsProcessor)
    {
        _integration = integration;
        _context = context;
    }

    protected override async Task InnerProcessContact(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcessLocation(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var response = await GetQuality(message);
        var generatedImage = GenerateImage(response);
        var respMessage = CreateMessage(message, response, generatedImage);

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, token);
    }

    private static Message CreateMessage(Message message, Response response, byte[] image)
    {
        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = "Air Quality",
            Body = response.Current?.EuropeanAqi.ToString(),
            Attachments = new List<IAttachment>()
            {
                new BinaryAttachment(Guid.NewGuid().ToString(), "air", MediaType.Image, string.Empty, image)
            }
        };
        return respMessage;
    }

    private async Task<Response> GetQuality(Message message)
    {
        var record = _context.UserLocationModels.FirstOrDefault(l => message.ChatIds.Contains(l.ChatId));
        if (record == default)
            return new Response
            {
                IsSuccess = false,
                Error = "Вы не ввели свою геолокацию",
                Latitude = 0,
                Longitude = 0,
                GenerationtimeMs = 0,
                UtcOffsetSeconds = 0,
                Timezone = null,
                TimezoneAbbreviation = null,
                Elevation = 0,
                HourlyUnits = null,
                Hourly = null,
                Current = null
            };

        var response = await _integration.GetAirQualityAsync(new Request
        {
            Latitude = record.Latitude,
            Longitude = record.Longitude,
            Current = "european_aqi",
            Hourly = "european_aqi"
        });
        
        return response;
    }

    private static byte[] GenerateImage(Response response)
    {
        var image = ImageUtils.PlaceText(@"Images\no_pollution.png",
            $"Воздух чистый... Но это не точно..." +
            $" AQI в вашем месте: {response.Current?.EuropeanAqi}", 60f,
            Color.Blue, 20, 40);
        return image;
    }
}