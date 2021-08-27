using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Credit.Model
{
    class Calculation
    {
        public List<Day> days;
        public List<string> filials;

        public delegate void DayListHandler();
        public event DayListHandler Notify;

        public Calculation(List<Day> days, List<string> filials, MainWindow parent)
        {
            Notify += () =>
            {
                foreach(Day day in days)
                {
                    parent.dataGrid_data.Columns[0].;
                }
            };
            this.days = days;
            this.filials = filials;
            foreach(string f in filials)
            {
                DataGridTextColumn d = new DataGridTextColumn();
                d.Header = f;
                d.Width = 50;
                parent.dataGrid_data.Columns.Add(d);
                parent.dataGrid_data.Columns[parent.dataGrid_data.Columns.Count - 1].IsReadOnly = false;
            }
            parent.dataGrid_data.MouseDoubleClick += (object sender, MouseButtonEventArgs args) =>
            {
                parent.label_credit.Content = args.Source.ToString();
                DataGrid source = (DataGrid)args.Source;
                if (source.CurrentColumn.DisplayIndex != 3) return;
                Day day = (Day)source.CurrentItem;
                if (day.Credit == 0) return;

            };

            Notify?.Invoke();

        }

        public void SetPersentFromDay(int day, double percent)
        {
            foreach (Day list_day in days)
                if (list_day.DayOfMonth >= day)
                    list_day.Percent = Convert.ToDecimal(percent);
            
            Notify?.Invoke();
            Calculate();
        }

        public void Calculate()
        {
            foreach (Day day in days)
            {
                day.Calculate();
            }
            Notify?.Invoke();
        }
    }
}
