using Botticelli.Framework.Controls.BasicControls;
using Botticelli.Framework.Controls.Layouts.Inlines;
using Botticelli.Framework.SendOptions;
using Botticelli.Framework.Telegram.Layout;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherQuality.Telegram.Commands.ReplyOptions;

public static class Replies
{
    private static IInlineTelegramLayoutSupplier _layoutSupplier = new InlineTelegramLayoutSupplier();

    public static SendOptionsBuilder<InlineKeyboardMarkup> GeneralReplyOptions
    {
        get
        {
            var markup = new InlineButtonMenu(2, 3);

            markup.AddControl(new Button
            {
                Content = "Details",
                CallbackData = "/Details"
            });

            markup.AddControl(new Button
            {
                Content = "Get quality",
                CallbackData = "/GetAirQuality"
            });

            // markup.AddControl(new Button
            // {
            //     Content = "Set location",
            //     CallbackData = "/SetLocation"
            // });

            var responseMarkup = _layoutSupplier.GetMarkup(markup);

            return SendOptionsBuilder<InlineKeyboardMarkup>.CreateBuilder(responseMarkup);
        }
    }
}