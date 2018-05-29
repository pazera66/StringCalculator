using System;
using System.Collections.Generic;
using System.Linq;
using SimpleStringCalculator.Interfaces;

namespace SimpleStringCalculator.Implementation
{
    // Pierwszy podejście do rozwiązania problemu
    public class StringCalculator1 : IStringCalculator1
    {
        private readonly IHelpers Helpers;
        private readonly List<char> Operators;

        public StringCalculator1(IHelpers helpers)
        {
            Helpers = helpers;
            Operators = Helpers.GetOperators;
        }

        public double PerformCalculations(string input)
        {
            Helpers.ValidateInput(input);

            //Tworzę tablicę samych liczb
            double[] numbersArray = input.Trim().Split(Operators.ToArray()).Select(x => Convert.ToDouble(x)).ToArray();

            var numberToAddOrSubstract = new List<double>();
            
            //Przeszukuję string w poszukiwaniu operatorów i wykonuję operacje na tablicy liczb traktując string jako maskę
            short i = 0;
            bool offset = false;
            double temp = 0;
            foreach (char c in input)
            {
                //Szukam operatorów
                if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    //Najpierw mnożenie i dzielenie
                    if (c == '*' || c == '/')
                    {
                        //Poruszając się odpowiednio po tablicy liczb wykonuję mnożenie lub dzielenie na konkretnych liczbach,
                        //których kolejność jest względna do liczby wystąpień operatorów
                        switch (c)
                        {
                            case '*':
                                temp = temp == 0 ? numbersArray[i] * numbersArray[i + 1] : temp * numbersArray[i + 1] ;
                                break;
                            case '/':
                                temp = temp == 0 ? numbersArray[i] / numbersArray[i + 1] : temp / numbersArray[i + 1];
                                break;
                        }

                        //Flaga potrzebna do łączenia wyników mnożenia i dzielenia do kolejnej operacji mnożenia i dzielenia
                        offset = true;
                    }
                    else
                    {
                        //Po natrafieniu pierwszego dodawania lub odejmowania wynik łączonych operacji mnożenia/dzielenia
                        //jest zapisywany na liście liczb do dodaniu lub odjęcia
                        if (offset)
                        {
                            offset = false;
                            numberToAddOrSubstract.Add(temp);
                            temp = 0;
                        }
                        else
                        {
                            numberToAddOrSubstract.Add(numbersArray[i]);
                        }
                    }

                    i++;
                }
            }
            //Obsługa przypadku gdy łączone mnożenie/dzielenie jest ostatnią operacją w stringu wejściowym
            if (offset)
            {
                numberToAddOrSubstract.Add(temp);
            }
            numberToAddOrSubstract.Add(numbersArray[i]);

            //Lista numberToAddOrSubstract zawiera teraz liczby które należy dodać/odjąć w pierwotnej kolejności
            // natomiast tam gdzie były inne operacje są liczby reprezentujące ich wyniki 1+2*2+3 => 1+4+3
            

            return PerformAdditionAndSubstraction(input, numberToAddOrSubstract);
        }

        private double PerformAdditionAndSubstraction(string input, List<double> numberToAddOrSubstract)
        {
            int i = 0;
            double result = numberToAddOrSubstract.First();
            foreach (char c in input)
            {
                if (c == '+' || c == '-')
                {
                    if (c == '+')
                    {
                        result += numberToAddOrSubstract[i + 1];
                    }
                    else
                    {
                        result -= numberToAddOrSubstract[i + 1];
                    }

                    i++;
                }
            }

            return result;
        }
    }
}
