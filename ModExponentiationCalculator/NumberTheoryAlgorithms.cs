using System;
using System.Collections.Generic;

namespace ModExponentiationCalculator
{
    public class NumberTheoryAlgorithms
    {
        /// <summary>
        /// Efficient algorithm for calculating modulo exponentiation
        /// i.e the remainder of (b raised to power(n)) / m
        /// </summary>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        public int ModExponentiation(int b, int n, int m)
        {
            var expansion = Base_b_Expansion(n, 2);

            int x = 1;
            var result = Div_Mod(b, m);
            int power = result.Mod;

            foreach (var i in expansion)
            {
                if (i == 1)
                {
                    result = Div_Mod(x * power, m);
                    x = result.Mod;
                }
                result = Div_Mod(power * power, m);
                power = result.Mod;
            }

            return x;
        }

        /// <summary>
        /// Base b expansion of integer n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="b"></param>
        public IEnumerable<int> Base_b_Expansion(int n, int b)
        {
            int quotient = n;
            List<int> expansion = new List<int>();

            while (quotient > 0)
            {
                var result = Div_Mod(quotient, b);
                expansion.Add(result.Mod);
                quotient = result.Div;

            }

            return expansion;
        }

        /// <summary>
        /// Calsulates the result and the remainder when n is divided by m
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public DivModResult Div_Mod(int n, int m)
        {
            int d = 0;
            int r = Math.Abs(n);

            while (r >= m)
            {
                r = r - m;
                d = d + 1;
            }

            if (n < 0 && r > 0)
            {
                r = m - r;
                d = -(d + 1);
            }

            DivModResult result = new DivModResult() { Div = d, Mod = r };

            return result;
        }
    }
}
