namespace UtilityBot.Services
{
    public interface IBotService
    {
        Task HandleStartCommandAsync(long userId);
        Task HandleTextMessageAsync(long userId, string message);
    }
}
