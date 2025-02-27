using System.Linq;
using UtilityBot.Utilities;

namespace UtilityBot.Services
{
    public class MessageHandler : IMessageHandler
    {
        private readonly TextAnalyzer _textAnalyzer;
        private readonly NumberCalculator _numberCalculator;

        public MessageHandler(TextAnalyzer textAnalyzer, NumberCalculator numberCalculator)
        {
            _textAnalyzer = textAnalyzer;
            _numberCalculator = numberCalculator;
        }

        public string HandleMessage(string message)
        {
            if (message.Contains(" "))
            {
                return _numberCalculator.CalculateSum(message);
            }
            else
            {
                return _textAnalyzer.CountCharacters(message);
            }
        }
    }
}
