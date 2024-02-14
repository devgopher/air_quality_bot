﻿using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Shared.ValueObjects;
using WeatherQuality.Telegram.Database;
using WeatherQuality.Telegram.Database.Models;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SetLocationProcessor : CommandProcessor<SetLocationCommand>
{
    private readonly WeatherQualityContext _context;
    
    public SetLocationProcessor(ILogger<SetLocationProcessor> logger, ICommandValidator<SetLocationCommand> validator, MetricsProcessor metricsProcessor, WeatherQualityContext context) 
        : base(logger, validator, metricsProcessor)
    {
        _context = context;
    }

    protected override async Task InnerProcessContact(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string args, CancellationToken token)  {
    }


    protected override async Task InnerProcessLocation(Message message, string args, CancellationToken token)
    {
        await _context.UserLocationModels.AddAsync(new UserLocationModel()
        {
            ChatId = message.ChatIds.FirstOrDefault(),
            Longitude = message.Location?.Longitude,
            Latitude = message.Location?.Latitude
        }, token);

        await _context.SaveChangesAsync(token);
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)  {
    }
}