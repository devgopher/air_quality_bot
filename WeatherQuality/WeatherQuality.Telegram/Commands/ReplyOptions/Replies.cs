using Botticelli.Framework.SendOptions;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherQuality.Telegram.Commands.ReplyOptions;

public static class Replies
{
    public static SendOptionsBuilder<ReplyMarkupBase> GeneralReplyOptions 
        => SendOptionsBuilder<ReplyMarkupBase>.CreateBuilder(new ReplyKeyboardMarkup(new[]
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