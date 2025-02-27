using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace UtilityBot
{
    class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;

        public Bot(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // receive all update types
                cancellationToken: stoppingToken);

            Console.WriteLine("Bot started");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                // Обработка нажатия на inline-кнопку
                string callbackData = update.CallbackQuery.Data;

                if (callbackData == "count_chars")
                {
                    await _telegramClient.SendTextMessageAsync(
                        update.CallbackQuery.From.Id,
                        "Вы выбрали подсчёт количества символов. Пожалуйста, отправьте текстовое сообщение.",
                        cancellationToken: cancellationToken);
                }
                else if (callbackData == "sum_numbers")
                {
                    await _telegramClient.SendTextMessageAsync(
                        update.CallbackQuery.From.Id,
                        "Вы выбрали вычисление суммы чисел. Пожалуйста, отправьте числа, разделённые пробелами.",
                        cancellationToken: cancellationToken);
                }

                // Уведомление о том, что запрос обработан
                await _telegramClient.AnswerCallbackQueryAsync(
                    update.CallbackQuery.Id,
                    "Выбор принят",
                    cancellationToken: cancellationToken);
            }
            else if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        string messageText = update.Message.Text;

                        if (messageText == "/start")
                        {
                            // Создаём inline-кнопки
                            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Подсчитать количество символов", "count_chars"),
                                    InlineKeyboardButton.WithCallbackData("Вычислить сумму чисел", "sum_numbers")
                                }
                            });

                            await _telegramClient.SendTextMessageAsync(
                                update.Message.From.Id,
                                "Привет! Выберите действие:",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);
                        }
                        else
                        {
                            // Проверка на наличие чисел в сообщении
                            var numbers = messageText.Split(' ')
                                                     .Where(word => int.TryParse(word, out _))
                                                     .Select(int.Parse)
                                                     .ToList();

                            if (numbers.Count == messageText.Split(' ').Length)
                            {
                                // Все слова - числа, вычисляем их сумму
                                int sum = numbers.Sum();
                                await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Сумма чисел: {sum}", cancellationToken: cancellationToken);
                            }
                            else
                            {
                                // Сообщение содержит текст, считаем количество символов
                                int length = messageText.Length;
                                await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"В вашем сообщении {length} символов.", cancellationToken: cancellationToken);
                            }
                        }
                        return;

                    default:
                        await _telegramClient.SendTextMessageAsync(update.Message.From.Id, "Данный тип сообщений не поддерживается. Пожалуйста, отправьте текст.", cancellationToken: cancellationToken);
                        return;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Waiting 10 seconds before retry");
            Thread.Sleep(10000);
            return Task.CompletedTask;
        }
    }
}
