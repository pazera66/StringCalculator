using NUnit.Framework;
using SimpleStringCalculator.Interfaces;
using Moq;
using System.Collections.Generic;
using SimpleStringCalculator.Implementation;

namespace CalculatorUnitTests
{
    [TestFixture]
    public class StringCalculator1UnitTests
    {
        private StringCalculator1 calc;
        private Mock<IHelpers> helpersMock;

        [SetUp]
        public void SetUp()
        {
            helpersMock = new Mock<IHelpers>();
            helpersMock.SetupGet(x => x.GetOperators).Returns(new List<char> { '/', '*', '+', '-' });
            calc = new StringCalculator1(helpersMock.Object);
        }

        [Test]
        public void SimpleAdd()
        {
            string input = "2+3";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void SimpleSubstract()
        {
            string input = "4-2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(2, result);
        }

        [Test]
        public void SimpleMultiply()
        {
            string input = "2*2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(4, result);
        }

        [Test]
        public void SimpleDivide()
        {
            string input = "10/2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void LongerAdd()
        {
            string input = "2+2+2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(6, result);
        }

        [Test]
        public void AddAndMultiplyCombined()
        {
            string input = "2+2*2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(6, result);
        }

        [Test]
        public void MultiplyNextToEachOther()
        {
            string input = "2*3*5";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(30, result);
        }

        [Test]
        public void LongCombo()
        {
            string input = "9/3+4/2*5-1-1-1*5";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(6, result);
        }

        [Test]
        public void Example1()
        {
            string input = "4+5*2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(14, result);
        }

        [Test]
        public void Example2()
        {
            string input = "4+5/2";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(6.5, result);
        }

        [Test]
        public void Example3()
        {
            string input = "4+5/2-1";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(5.5, result);
        }

        [Test]
        public void VeryLongInputString()
        {
            string input = "4+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-14+5/2+7*15+24+14-48*75/18-14+5/2+7*15+24+14-48*75/18-14+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-14+5/2+7*15+24+14-48*75/18-1+4+5/2+7*15+24+14-48*75/18-1";

            double result = calc.PerformCalculations(input);

            Assert.AreEqual(-686, result);
        }
    }
}