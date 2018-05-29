using NUnit.Framework;
using SimpleStringCalculator;
using System;
using SimpleStringCalculator.Interfaces;
using Moq;
using System.Collections.Generic;
using SimpleStringCalculator.Implementation;

namespace CalculatorUnitTests
{
    [TestFixture]
    public class HelpersUnitTests
    {
        private IHelpers Helpers;

        [SetUp]
        public void SetUp()
        {
            Helpers = new Helpers();
        }

        [Test]
        public void InvalidInput_FirstCharIsOperator()
        {
            string input = "+1+2";
            Exception expectedException = null;

            try
            {
                Helpers.ValidateInput(input);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("Wprowadzony string wejściowy jest nieprawidłowy. Operator znajduje się na poczatku lub końcu stringa.", expectedException.Message);
        }

        [Test]
        public void InvalidInput_TwoOperatorsNextToEachOther()
        {
            string input = "1++1";
            Exception expectedException = null;

            try
            {
                Helpers.ValidateInput(input);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            Assert.IsNotNull(expectedException);
            Assert.AreEqual("Wprowadzony string wejściowy jest nieprawidłowy! Dwa operatory występują po sobie bezpośrednio!", expectedException.Message);
        }
    }
}