using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Model
{
    class Day
    {
        public int DayOfMonth
        {
            get;
            private set;
        }

        public decimal Balance
        {
            get;
            private set;
        }
        // public double Percent;
        public FilialNode filialNode
        {
            get;
            set;
        }

        public Day(decimal balance, string date)
        {
            Balance = balance;
            DayOfMonth = Convert.ToInt32(date);
        }

        public bool IfIssuance (Day previous)
        {
            return previous.Balance < Balance;
        }

        public override string ToString()
        {
            return $"{ Balance } { filialNode }";
        }
    }
}
