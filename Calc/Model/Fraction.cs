using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Calc.Extensions;

namespace Calc.Model
{
    public class Fraction
    {
        private uint numerator;
        private uint denominator;

        public Fraction() { }

        public Fraction(uint n, uint d)
        {
            numerator = n;
            denominator = d;
            Simplify();
        }

        public Fraction(decimal d)
        {
            if (d == 1.0M)
            {
                numerator = 1;
                denominator = 1;
                return;
            }

            numerator = Convert.ToUInt32(d * 1000);
            denominator = 1000;
            Simplify();
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Fraction operator +(Fraction a, decimal b)
        {
            b *= 1000;
            uint ui = Convert.ToUInt32(b);
            return new Fraction(a.numerator + ui * a.denominator, a.denominator * 1000);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator - b.numerator * a.denominator, a.denominator * b.denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Fraction operator *(Fraction a, decimal b)
        {
            b *= 1000;
            uint ui = Convert.ToUInt32(b);
            return new Fraction(a.numerator * ui, a.denominator * 1000);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }
        public static Fraction operator /(Fraction a, decimal b)
        {
            b *= 1000;
            uint ui = Convert.ToUInt32(b);
            return new Fraction(a.numerator * 1000, a.denominator * ui);
        }

        public static implicit operator double(Fraction a)
        {
            return (double)a.numerator / a.denominator;
        }

        public override string ToString()
        {
            return $"{ numerator }/{ denominator }";
        }


        private void Simplify()
        {
            uint gcd_ab = Gcd(numerator, denominator);
            numerator = numerator / gcd_ab;
            denominator = denominator / gcd_ab;
            if (denominator == 0)
                denominator = 1;
        }

        public static uint Gcd(uint value1, uint value2)
        {
            while (value1 != 0)
            {
                uint t = value2 % value1;
                value2 = value1;
                value1 = t;
            }
            return value2;
        }


        public void BecomeEasier()
        {
            List<uint> same = null;
            Dictionary<uint, short> numer, denom;
            numer = Factorization(numerator);
            denom = Factorization(denominator);

            foreach (uint num in numer.Keys.Intersect(denom.Keys).ToList())
            {
                while(numer[num] > 0 && denom[num] > 0)
                {
                    numer[num]--;
                    denom[num]--;
                }
            }

            foreach (uint key in numer.Keys.ToList())
                if (numer[key] == 0)
                    numer.Remove(key);
            foreach (uint key in denom.Keys.ToList())
                if (denom[key] == 0)
                    denom.Remove(key);

            Func<KeyValuePair<uint, short>, uint> func = (pair) =>
            {
                switch (pair.Value)
                {
                    case 0:
                        return 0;
                    default:
                        uint result = 1;
                        for (short count = 0; count < pair.Value; count++)
                            result *= pair.Key;
                        return result;
                }
            };


            numerator = numer.Sum(func);
            denominator = denom.Sum(func);

            if (denominator == 0)
                denominator = 1;
        }

        public Dictionary<uint, short> Factorization(uint num)
        {
            if (num == 1)
                return new Dictionary<uint, short> { { 1, 1 } };
            
            Dictionary<uint, short> result = new Dictionary<uint, short>();

            while ((num % 2) == 0 && num != 0)
            {
                num = num / 2;
                if (result.ContainsKey(2))
                    result[2]++;
                else
                    result[2] = 1;
            }
            
            for (uint i = 3; i <= num;)
            {
                if (num % i == 0)
                {
                    if (result.ContainsKey(i))
                        result[i]++;
                    else
                        result[i] = 1;
                    num /= i;
                }
                else
                {
                    i += 2;
                }
            }

            return result;
        }

    }
}
