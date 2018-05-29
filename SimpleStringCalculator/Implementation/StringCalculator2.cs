using System;
using System.Collections.Generic;
using System.Linq;
using SimpleStringCalculator.Interfaces;

namespace SimpleStringCalculator.Implementation
{
    public class StringCalculator2 : IStringCalculator2
    {
        private readonly IHelpers Helpers;
        private readonly List<char> Operators;

		//To jest drugi wymyślony przeze mnie algorytm. Jest on jednocześnie lepiej dopracowany od pierwszego
        public StringCalculator2(IHelpers helpers)
        {
            Helpers = helpers;
            Operators = Helpers.GetOperators;
        }

        public double PerformCalculations(string input)
        {
            Helpers.ValidateInput(input);

            //Operacje wykonywane są w optymalny sposób.
            //Skoro nie ma nawiasów to dzielenie można wykonać jako pierwsze, ponieważ dzielna zawsze jest jedną liczbą
            try
            {
                input = PerformCalculationForSpecificOperator(input, '/');
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            input = PerformCalculationForSpecificOperator(input, '*');

            //Należy przetworzyć string w celu zamiany odejmowania na dodawanie wartości ujemnych
            input = Helpers.ConvertSubstractionToAdditionNegativeValues(input);
            input = PerformCalculationForSpecificOperator(input, '+');

            double result = input[0].Equals('|')
                ? Convert.ToDouble(input.TrimStart('|')) * -1
                : Convert.ToDouble(input);

            //Dokładność wyniku do 3 miejsca po przecinku
            return Math.Round(result, 3);
        }

        private string PerformCalculationForSpecificOperator(string input, char operatorChar)
        {
            //Dzielę cały input na dwa substring i wykonuję operację dla ostatniego elementu
            //będącego liczbą w pierwszym substringu i analogicznie z pierwszym elementem w drugim substringu
            var splitForDivision = input.Split(new[] { operatorChar }, 2).ToList();

            //Jeśli nie nastąpił split to oznacza, że nie ma już operacji danego typu do wykonania
            //Następuje przerwanie rekurencji
            if (splitForDivision.Count == 1)
            {
                return input;
            }

            var leftSubstring = splitForDivision[0];
            var rightSubstring = splitForDivision[1];
            string value1AsString = leftSubstring.Split(Operators.ToArray()).Last();
            string value2AsString = rightSubstring.Split(Operators.ToArray()).First();

            //Obsługa wartości ujemnych oznaczonych przedrostkiem '\' w celu odróżnienia ich od operatora odejmowania
            var value1 = value1AsString[0].Equals('|')
                ? Convert.ToDouble(value1AsString.TrimStart('|')) * -1
                : Convert.ToDouble(value1AsString);

            var value2 = value2AsString[0].Equals('|')
                ? Convert.ToDouble(value2AsString.TrimStart('|')) * -1
                : Convert.ToDouble(value2AsString);

            double result = 0;
            bool resolveConflictBetweenNegativeResultAndOperators = false;

            //Wykonuję odpowiednią operację zależnie od operatora
            switch (operatorChar)
            {
                case '/':
                    if (value2.Equals(0)) 
                    {
                        throw new ArgumentException("Can't divide by 0");
                    }
                    result = Math.Round(value1 / value2, 3);
                    break;
                case '*':
                    result = value1 * value2;
                    break;
                case '+':
                    result = value1 + value2;
                    if (result < 0)
                    {
                        resolveConflictBetweenNegativeResultAndOperators = true;
                    }
                    break;
            }

            //W pierwszym substringu usuwam ostatnia liczbę i dodaję wynik operacji
            var indexOfValue1 = leftSubstring.LastIndexOf(value1AsString, StringComparison.InvariantCulture);
            if (resolveConflictBetweenNegativeResultAndOperators)
            {
                leftSubstring = $"{leftSubstring.Remove(indexOfValue1)}|{result * -1}";
            }
            else
            {
                leftSubstring = leftSubstring.Remove(indexOfValue1) + result;
            }

            //Znajduję pierwszy operator w drugim substringu i de fakto wycinam z niego pierwsza liczbę
            int[] indexesOfFirstOccurencesOfEachOperators = new int[4];
            for (int i = 0; i < 4; i++)
            {
                indexesOfFirstOccurencesOfEachOperators[i] = rightSubstring.IndexOf(Operators[i]);
            }

            //Zapamiętuję pod uwagę tylko indeksy istniejących w stringu operatorów
            var validIndexesOfOperators = indexesOfFirstOccurencesOfEachOperators.Where(x => x >= 0).ToList();
            var indexOfFirstOperator = validIndexesOfOperators.Any() ? validIndexesOfOperators.Min() : 0;

            //Jeśli brak jest operatorów to oznacza, to że w stringu jest tylko liczba.
            rightSubstring = indexOfFirstOperator == 0 ? string.Empty : rightSubstring.Substring(indexOfFirstOperator);

            //Łącze oba string w jeden i dalej przetwarzam je rekurencyjnie 
            return PerformCalculationForSpecificOperator(leftSubstring + rightSubstring, operatorChar);
        }
    }
}
