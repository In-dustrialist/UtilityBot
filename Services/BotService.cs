using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Utilities;
using Telegram.Bot.Types.ReplyMarkups;

namespace UtilityBot.Services
{
    public class BotService : IBotService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IMessageHandler _messageHandler;

        public BotService(ITelegramBotClient telegramBotClient, IMessageHandler messageHandler)
        {
            _telegramBotClient = telegramBotClient;
            _messageHandler = messageHandler;
        }

        public async Task HandleStartCommandAsync(long userId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton { Text = "Подсчёт символов", CallbackData = "char_count" },
                new InlineKeyboardButton { Text = "Вычисление суммы", CallbackData = "sum_numbers" }
            });

            await _telegramBotClient.SendTextMessageAsync(userId, "Привет! Выберите действие:", replyMarkup: keyboard);
        }

        public async Task HandleTextMessageAsync(long userId, string message)
        {
            var response = _messageHandler.HandleMessage(message);
            await _telegramBotClient.SendTextMessageAsync(userId, response);
        }
    }
}
