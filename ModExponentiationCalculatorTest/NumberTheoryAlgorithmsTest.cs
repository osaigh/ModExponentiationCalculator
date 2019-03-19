using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModExponentiationCalculator;

namespace ModExponentiationCalculatorTest
{
    [TestClass]
    public class NumberTheoryAlgorithmsTest
    {
        [TestMethod]
        public void ModExponentiation_BaseOperation_Expected36()
        {
            NumberTheoryAlgorithms modCalculator = new NumberTheoryAlgorithms();

            var result = modCalculator.ModExponentiation(3,644,645);

            Assert.AreEqual(36, result);
        }
    }
}
