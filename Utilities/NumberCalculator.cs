using System.Linq;

namespace UtilityBot.Utilities
{
    public class NumberCalculator
    {
        public string CalculateSum(string message)
        {
            var numbers = message.Split(" ")
                                 .Select(x => int.TryParse(x, out var n) ? n : 0)
                                 .ToList();
            var sum = numbers.Sum();
            return $"Сумма чисел: {sum}";
        }
    }
}
