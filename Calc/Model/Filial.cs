using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Model
{
    class Filial
    {
        private string name;

        public Filial(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
