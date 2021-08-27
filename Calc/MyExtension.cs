using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Extensions
{
    static class MyExtension
    {
        public static uint Sum(this IEnumerable<KeyValuePair<uint, short>> source, Func<KeyValuePair<uint, short>, uint> func)
        {
            uint sum = 0;
            foreach (var number in source)
            {
                sum *= func(number);
            }
            return sum;
        }

        public static IEnumerable<KeyValuePair<uint, short>> Select(this IEnumerable<KeyValuePair<uint, short>> source, Func<KeyValuePair<uint, short>, uint> func)
        {

            return null;
        }
    }
}
