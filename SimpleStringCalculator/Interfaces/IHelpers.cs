using System.Collections.Generic;

namespace SimpleStringCalculator.Interfaces
{
    public interface IHelpers
    {
        List<char> GetOperators { get; }
        void ValidateInput(string input);
        string ConvertSubstractionToAdditionNegativeValues(string input);
    }
}