using Botticelli.Bot.Interfaces.Agent;
using Botticelli.Bot.Interfaces.Bus.Handlers;
using Botticelli.Bot.Interfaces.Client;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.API.Client.Responses;


namespace WeatherQuality.Telegram.Bus;

public class AirQualityRequestHandler : IHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly ILogger<AirQualityRequestHandler> _logger;
    private readonly IBusClient _bus;
    
    public AirQualityRequestHandler(ILogger<AirQualityRequestHandler> logger, IBusClient bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task Handle(SendMessageRequest input, CancellationToken token)
    {
        _logger.LogDebug("Handled");
        await _bus.SendResponse(new SendMessageResponse(input.Uid, string.Empty), token);
    }
}