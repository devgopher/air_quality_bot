using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherQuality.Infrastructure;
using WeatherQuality.Infrastructure.Models;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SetLocationProcessor : CommandProcessor<SetLocationCommand>
{
    private readonly WeatherQualityContext _context;
    private readonly SendOptionsBuilder<ReplyMarkupBase> _options;
    
    public SetLocationProcessor(ILogger<SetLocationProcessor> logger, ICommandValidator<SetLocationCommand> validator, MetricsProcessor metricsProcessor, WeatherQualityContext context) 
        : base(logger, validator, metricsProcessor)
    {
        _context = context;
        _options = SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
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

    protected override async Task InnerProcessContact(Message message, string args, CancellationToken token)
    {
    }

    protected override async Task InnerProcessPoll(Message message, string args, CancellationToken token)  {
    }


    protected override async Task InnerProcessLocation(Message message, string args, CancellationToken token)
    {
        var entity = await _context
                           .UserLocationModels
                           .FirstOrDefaultAsync(e => e.ChatId ==
                                                     message
                                                             .ChatIds
                                                             .FirstOrDefault(),
                                                token);

        if (entity == null)
        {
            await _context.UserLocationModels.AddAsync(new UserLocationModel
                                                       {
                                                           ChatId = message.ChatIds.FirstOrDefault()!,
                                                           Longitude = (float)message.Location?.Longitude!,
                                                           Latitude = (float)message.Location?.Latitude!
                                                       },
                                                       token);
        }
        else
        {
            entity.Latitude = (float)message.Location?.Latitude!;
            entity.Longitude = (float)message.Location?.Longitude!;

            _context.Update(entity);
        }
        
        await _context.SaveChangesAsync(token);
        
        await _bot.SendMessageAsync(new SendMessageRequest(message.Uid)
        {
            Message = new Message
            {
                Uid = message.Uid,
                ChatIds = message.ChatIds,
                Subject = "Location received...",
                Body = "Now you may get air quality"
            }
        }, _options, token);
    }

    protected override async Task InnerProcess(Message message, string args, CancellationToken token)  {
    }
}