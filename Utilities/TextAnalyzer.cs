namespace UtilityBot.Utilities
{
    public class TextAnalyzer
    {
        public string CountCharacters(string message)
        {
            return $"В вашем сообщении {message.Length} символов.";
        }
    }
}
