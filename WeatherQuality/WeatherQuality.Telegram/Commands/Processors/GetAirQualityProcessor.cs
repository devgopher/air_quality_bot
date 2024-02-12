using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using WeatherQuality.Domain.Request;
using WeatherQuality.Integration;

namespace WeatherQuality.Telegram.Commands.Processors;

public class GetAirQualityProcessor : CommandProcessor<GetAirQualityCommand>
{
    private readonly IIntegration _integration;
    
    
    public GetAirQualityProcessor(ILogger logger, ICommandValidator<GetAirQualityCommand> validator, MetricsProcessor metricsProcessor, IIntegration integration) : base(logger, validator, metricsProcessor)
    {
        _integration = integration;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override Task InnerProcessLocation(Message message, string args, CancellationToken token) => throw new NotImplementedException();

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)
    {
        var response = await _integration.GetAirQualityAsync(new Request
        {
            Latitude = 0,
            Longitude = 0,
            Current = null,
            Hourly = null,
            Timezone = 
        });

        var respMessage = new Message
        {
            Uid = message.Uid,
            ChatIds = message.ChatIds,
            Subject = "Air Quality",
            Body = response.,
            Attachments = null,
            From = null,
            ForwardedFrom = null,
            Contact = null,
            Poll = null,
            ReplyToMessageUid = null,
            Location = null
        };

        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid), token);
    }
}