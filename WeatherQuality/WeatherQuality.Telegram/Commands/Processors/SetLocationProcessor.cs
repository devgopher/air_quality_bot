using Botticelli.Client.Analytics;
using Botticelli.Framework.Commands.Processors;
using Botticelli.Framework.Commands.Validators;
using Botticelli.Framework.Controls.BasicControls;
using Botticelli.Framework.Controls.Layouts;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.Controls.Parsers;
using Botticelli.Framework.SendOptions;
using Botticelli.Shared.API.Client.Requests;
using Botticelli.Shared.ValueObjects;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherQuality.Infrastructure;
using WeatherQuality.Infrastructure.Models;
using WeatherQuality.Telegram.Commands.ReplyOptions;

namespace WeatherQuality.Telegram.Commands.Processors;

public class SetLocationProcessor : CommandProcessor<SetLocationCommand>
{
    private readonly WeatherQualityContext _context;
    private readonly SendOptionsBuilder<InlineKeyboardMarkup> _options;
    
    public SetLocationProcessor(ILogger<SetLocationProcessor> logger,
                                ICommandValidator<SetLocationCommand> validator, 
                                MetricsProcessor metricsProcessor,
                                WeatherQualityContext context) 
        : base(logger, validator, metricsProcessor)
    {
        _context = context;
        _options = Replies.GeneralReplyOptions;
    }

    protected override Task InnerProcessContact(Message message, string args, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task InnerProcessPoll(Message message, string args, CancellationToken token)
    {
        return Task.CompletedTask;
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

        try
        {
            await Bot.SendMessageAsync(new SendMessageRequest(message.Uid)
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
        catch (FlurlHttpException ex)
        {
            // Todo: logging
        }
    }

    protected override Task InnerProcess(Message message, string args, CancellationToken token)
    {
        return Task.CompletedTask;
    }
}