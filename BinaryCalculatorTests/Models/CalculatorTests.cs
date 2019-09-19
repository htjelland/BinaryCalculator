using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCalculator.Models.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        [TestMethod()]
        public void AdditionTest()
        {
            int first = 3;
            int second = 2;
            var operation = "+";

            var calculator = new Calculator();
            var result = calculator.Calculate(first, second, operation);
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        public void SubtractionTest()
        {
            int first = 3;
            int second = 2;
            var operation = "-";

            var calculator = new Calculator();
            var result = calculator.Calculate(first, second, operation);
            Assert.AreEqual(1, result);
        }

    }
}