using Calc.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Excel = Microsoft.Office.Interop.Excel;

namespace Calc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
        private List<Filial> filials = new List<Filial>
        {
            new Filial("f1"),
            new Filial("f2"),
            new Filial("f3"),
            new Filial("f4")
        };
        */
        private Calculation mainCalculation;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                label_drag.Content = "Release";
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void DragLeave(object sender, DragEventArgs e)
        {
            label_drag.Content = "Drag & Drop";
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            label_drag.Content = "Drag & Drop";
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ReadProcess(files[0].ToString());
        }

        private void ReadProcess(string file_path)
        {
            Excel.Application application = new Excel.ApplicationClass();

            Excel.Workbook workbook = application.Workbooks.Open(
                file_path,
                Type.Missing, true, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing
            );
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            Excel.Worksheet sheet2 = (Excel.Worksheet)workbook.Sheets[2];

            var columns = new
            {
                Date = 1,
                Debet = 5,
                Credit = 6,
                Balance = 8
            };
            
            List<Day> list = new List<Day>();
            FilialNode filialNode = null;

            string date;
            Excel.Range bal, dat;
            double balance;
            for (int row = 11; ((Excel.Range)sheet1.Cells[row + 1, columns.Balance]).Value2 != null; row++)
            {
                dat = (Excel.Range)sheet1.Cells[row, columns.Date];
                bal = (Excel.Range)sheet1.Cells[row, columns.Balance];

                date = dat.Value2 == null ? "" : (string)dat.Value2;
                balance = bal.Value2 == null ? 0 : (double)bal.Value2;

                date = date.Split(' ')[2];

                Day day = new Day((decimal)balance, date);
                list.Add(day);

                dat = null;
                bal = null;
            }

            mainCalculation = new Calculation(list);

            // remove

            decimal[][] mas = new decimal[][]
            {
                new decimal[]{ 0.25M, 0.125M, 0.375M, 0.25M },
                new decimal[]{ 0.25M, 0.125M, 0.375M, 0.25M },
                new decimal[]{ 0.2M, 0.25M, 0.25M, 0.3M }
            };
            
            int i = 0;
            List<string> filials = new List<string>();

            for (int col = 2; ((Excel.Range)sheet2.Cells[1, col]).Value2 != null; col++)
                filials.Add((string)((Excel.Range)sheet2.Cells[1, col]).Value2);

            for (int row = 2; ((Excel.Range)sheet2.Cells[row, 1]).Value2 != null; row++)
            {
                int dayInt = Convert.ToInt32(((Excel.Range)sheet2.Cells[row, 1]).Value2);
                Day day = mainCalculation.findDayByDay(dayInt);
                day.filialNode = new FilialNode(filials);

                decimal sum = 0;
                for (int col = 2; ((Excel.Range)sheet2.Cells[2, col]).Value2 != null; col++)
                {
                    decimal d = Convert.ToDecimal(((Excel.Range)sheet2.Cells[row, col]).Value2);
                    sum += d;
                    day.filialNode.distribution[(string)((Excel.Range)sheet2.Cells[1, col]).Value2] = new Fraction(d);
                }
                foreach (string key in day.filialNode.distribution.Keys.ToList())
                {
                    day.filialNode.distribution[key] = day.filialNode.distribution[key] / sum;
                }
            }

            mainCalculation.Calculate(this);
            workbook.Close();
        }
    }
}
