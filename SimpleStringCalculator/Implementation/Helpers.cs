using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleStringCalculator.Interfaces;

namespace SimpleStringCalculator.Implementation
{
    public class Helpers : IHelpers
    {
        public List<char> GetOperators { get; } = new List<char> { '/', '*', '+', '-' };

        public void ValidateInput(string input)
        {
            char firstChar = input[0];
            char lastChar = input[input.Length - 1];

            bool isFirstInputCharAnOperator = GetOperators.Any(x => x.Equals(firstChar));
            bool isLastInputCharAnOperator = GetOperators.Any(x => x.Equals(lastChar));

            if (isFirstInputCharAnOperator || isLastInputCharAnOperator)
            {
                throw new ArgumentException("Wprowadzony string wejściowy jest nieprawidłowy. Operator znajduje się na poczatku lub końcu stringa.");
            }

            var charArray = input.ToArray();
            for (int i = 1; i < charArray.Length - 2; i++)
            {
                if (GetOperators.Contains(charArray[i]))
                {
                    if (GetOperators.Contains(charArray[i + 1]))
                    {
                        throw new ArgumentException("Wprowadzony string wejściowy jest nieprawidłowy! Dwa operatory występują po sobie bezpośrednio!");
                    }
                }
            }
        }

        public string ConvertSubstractionToAdditionNegativeValues(string input)
        {
            //Robię split po znaku 'minus' np 100-10+5 => [100] [10+5]
            var splitForDivision = input.Split('-').ToList();
            StringBuilder builder = new StringBuilder();
            builder.Append(splitForDivision[0]);

            for (int i = 1; i < splitForDivision.Count; i++)
            {
                //Dzielę string aby wyciągnąc pierwszą liczbę [10,5] => 10, 5
                //Następnie pierwsza liczba jest dodawana do StringBuildera ze znacznikiem '|' określającym że jest ujemna
                //Na końcu dodaję pozostałość substringa
                //Całość 100-10+5 => [100] [10,5] => 100  +|10 +5 => 100+|10+5
                var subString = splitForDivision[i].Split('+');
                builder.Append("+|" + subString.First());
                for (int j = 1; j < subString.Length; j++)
                {
                    builder.Append($"+{subString[j]}");
                }
            }

            return builder.ToString();
        }
    }
}
