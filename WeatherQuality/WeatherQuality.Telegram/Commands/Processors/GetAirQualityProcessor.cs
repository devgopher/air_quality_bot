using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.Constants;
using Botticelli.Shared.ValueObjects;
using SixLabors.ImageSharp;
using WeatherQuality.Domain.Request;
using WeatherQuality.Integration;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : CommandProcessor<GetAirQualityCommand>
{
    private readonly IIntegration _integration;


    public GetAirQualityProcessor(ILogger logger, ICommandValidator<GetAirQualityCommand> validator,
        MetricsProcessor metricsProcessor, IIntegration integration) : base(logger, validator, metricsProcessor)
    {
        _integration = integration;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) =>
        throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var response = await _integration.GetAirQualityAsync(new Request
        {
            Latitude = message.Location?.Latitude,
            Longitude = message.Location?.Longitude,
            Current = "european_aqi",
            Hourly = "european_aqi"
        });

        var image = ImageUtils.PlaceText(@"Images\no_pollution.png", "Воздух чистый... Но это не точно...", 15f,
            Color.Blue, 20, 40);

        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = "Air Quality",
            Body = response.Hourly.Current?.EuropeanAqi.ToString(),
            Attachments = new List<IAttachment>()
            {
                new BinaryAttachment(Guid.NewGuid().ToString(), "air", MediaType.Image, string.Empty, image)
            }
        };

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = respMessage
        }, token);
    }
}