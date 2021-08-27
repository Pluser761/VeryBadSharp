using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Model
{
    class Calculation
    {
        public List<Day> days;

        public Calculation(List<Day> days)
        {
            this.days = days;
        }

        public void Calculate(MainWindow parent)
        {
            foreach (KeyValuePair<string, Fraction> filialPropotion in days[0].filialNode.distribution)
            {
                parent.textBox.Text += filialPropotion.Key + " ";
            }
            parent.textBox.Text += "\n";

            /*
            foreach (Day day in days)
            {
                foreach (KeyValuePair<string, Fraction> filialPropotion in day.filialNode.distribution)
                {
                    parent.textBox.Text += (double)(filialPropotion.Value * day.Balance) + " ";
                }
                parent.textBox.Text += "\n";
            }
            parent.textBox.Text += "\n";
            */


            foreach (KeyValuePair<string, Fraction> filialPropotion in days[0].filialNode.distribution)
            {
                parent.textBox.Text += (double)(filialPropotion.Value * days[0].Balance) + " ";
            }
            parent.textBox.Text += "\n";
            for (int i = 1; i < days.Count; i++)
            {
                if (days[i].Balance > days[i - 1].Balance)
                {
                    // пересчет
                    foreach (string filialPropotionKey in days[i].filialNode.distribution.Keys.ToList())
                    {
                        days[i].filialNode.distribution[filialPropotionKey] = (days[i - 1].filialNode.distribution[filialPropotionKey] * days[i - 1].Balance + days[i].filialNode.distribution[filialPropotionKey] * (days[i].Balance - days[i - 1].Balance)) / days[i].Balance;
                    }
                }
                foreach (KeyValuePair<string, Fraction> filialPropotion in days[i].filialNode.distribution)
                {
                    parent.textBox.Text += (double)(filialPropotion.Value * days[i].Balance) + " ";
                }
                parent.textBox.Text += "\n";
            }

            return;
            /*
            foreach (KeyValuePair<Filial, Fraction> filialPropotion in days[0].filialNode.distribution)
            {
                Fraction val = filialPropotion.Value * days[0].Balance;
                parent.textBox.Text += val + " ";
            }
            parent.textBox.Text += "\n";
            for (int i = 1; i < days.Count; i++)
            {
                if (days[i].IfIssuance(days[i - 1]))
                {
                    Dictionary<Filial, Fraction> newFilialPropotion = days[i].filialNode.distribution.ToDictionary(entry => entry.Key, entry => entry.Value);
                    foreach (KeyValuePair<Filial, Fraction> filialPropotion in days[i].filialNode.distribution)
                    {
                        newFilialPropotion[filialPropotion.Key] = (filialPropotion.Value * (days[i].Balance - days[i - 1].Balance) + days[i - 1].filialNode.distribution[filialPropotion.Key] * days[i - 1].Balance) / days[i].Balance;
                    }
                    days[i].filialNode.distribution = newFilialPropotion;
                }
                foreach (KeyValuePair<Filial, Fraction> filialPropotion in days[i].filialNode.distribution)
                {
                    Fraction val = filialPropotion.Value * days[i].Balance;
                    parent.textBox.Text += val + " ";
                }
                parent.textBox.Text += "\n";
            }
            */
        }

        public Day findDayByDay(int day)
        {
            if (day <= days.Count)
            {
                int i = 0;
                while(day != days[day - 1 + i].DayOfMonth)
                {
                    if (days[day + i].DayOfMonth > day)
                        i--;
                    else
                        i++;
                }
                return days[day - 1 + i];
            }
            else throw new Exception("Key error");
        }
    }
}
