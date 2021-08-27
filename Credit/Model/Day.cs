using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credit.Model
{
    class Day
    {
        private CultureInfo provider = CultureInfo.InvariantCulture;
        private DateTime _date;
        private Credit _credit = new Credit();
        private decimal _percent = 0;
        private decimal _amount = 0;
        private Dictionary<string, decimal> _filialDole = new Dictionary<string, decimal>();

        public decimal Debet { get; private set; }
        public decimal Balance { get; private set; }

        public string Date {
            get => _date.Day.ToString();
            private set
            {
                _date = DateTime.ParseExact(value, "dd.MM.yy", provider);
            }
        }
        public int DayOfMonth
        {
            get => _date.Day;
        }
        public decimal Amount {
            get => _amount;
            set => _amount = Math.Round(value, 2);
        }
        public decimal Percent {
            get => _percent;
            set => _percent = Convert.ToDecimal(value);
        }
        public decimal Credit
        {
            get => _credit.Amount;
            private set => _credit.Amount = value;
        }

        public Day(string date, double cred, double deb, double ost)
        {
            Date = date;
            Credit = Convert.ToDecimal(cred);
            Debet = Convert.ToDecimal(deb);
            Balance = Convert.ToDecimal(ost);
        }

        public void Calculate()
        {
            Amount = Balance * Percent / 36500;
        }


        public override string ToString()
        {
            return $"{DayOfMonth} число. Выдача {Credit}";
        }
    }
}
