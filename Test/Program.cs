using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Calc.Model;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal d = 0.375M;
            Fraction fr;
            Fraction fraction = new Fraction(d);
            Console.WriteLine(fraction);

            fr = fraction * fraction;
            Console.WriteLine(fr);

            fr = fraction * fr;
            Console.WriteLine(fr);
            Console.ReadKey();
        }
    }
}
