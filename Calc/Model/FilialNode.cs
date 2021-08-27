using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Model
{
    class FilialNode
    {
        public FilialNode previous { get; }

        public Dictionary<string, Fraction> distribution = new Dictionary<string, Fraction>();

        public FilialNode(List<string> filials, FilialNode previous = null)
        {
            // remove
            foreach (string filial in filials)
                distribution.Add(filial, new Fraction());
            this.previous = previous;
        }

        public override string ToString()
        {
            string s = "";
            foreach (KeyValuePair<string, Fraction> pair in distribution)
                s += $"{pair.Value}";
            return s;
        }
    }
}
