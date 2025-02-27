using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using UtilityBot.Services;
using UtilityBot.Utilities;

namespace UtilityBot
{
    static class Program
    {
        public static string Token = "YOUR_BOT_TOKEN"; //Укажите Ваш ТОКЕН 

        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Starting Service");
            await host.RunAsync();
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(Token));
            services.AddSingleton<TextAnalyzer>();
            services.AddSingleton<NumberCalculator>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddSingleton<IBotService, BotService>();
            services.AddHostedService<Bot>();
        }
    }
}
