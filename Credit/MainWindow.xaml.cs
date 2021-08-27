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
using Credit.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Credit
{
    public partial class MainWindow : Window
    {
        private Calculation mainCalculation;
        private List<string> filials = new List<string>
        {
            "ф1",
            "ф2",
            "ф3",
            "ф4",
            "ф5",
            "ф6",
            "ф7"
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Percent(object sender, RoutedEventArgs e)
        {
            mainCalculation.SetPersentFromDay(Convert.ToInt32(textBox_day.Text), Convert.ToDouble(textBox_percent.Text));
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
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
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];


            var columns = new
            {
                Date = 1,
                Debet = 5,
                Credit = 6,
                Balance = 8
            };

            List<Day> list = new List<Day>();
            Excel.Range cred, deb, bal, dat;
            double credit, debet, balance;
            string date;
            for (int row = 11; ((Excel.Range)sheet.Cells[row + 1, columns.Balance]).Value2 != null; row++)
            {
                dat = (Excel.Range)sheet.Cells[row, columns.Date];
                cred = (Excel.Range)sheet.Cells[row, columns.Credit];
                deb = (Excel.Range)sheet.Cells[row, columns.Debet];
                bal = (Excel.Range)sheet.Cells[row, columns.Balance];

                date = dat.Value2 == null ? "" : (string)dat.Value2;
                credit = cred.Value2 == null ? 0 : (double)cred.Value2;
                debet = deb.Value2 == null ? 0 : (double)deb.Value2;
                balance = bal.Value2 == null ? 0 : (double)bal.Value2;

                date = date.Split(' ')[2];
                Day day = new Day(date, credit, debet, balance);
                list.Add(day);

                dat = null;
                cred = null;
                deb = null;
                bal = null;
            }

            // return list;
            mainCalculation = new Calculation(list, filials, this);
        }

        private void Label_drag_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                label_drag.Content = "Release";
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void Label_drag_DragLeave(object sender, DragEventArgs e)
        {
            label_drag.Content = "Drag & Drop";
        }

        private void Label_drag_Drop(object sender, DragEventArgs e)
        {
            label_drag.Content = "Drag & Drop";
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ReadProcess(files[0].ToString());
        }
    }
}
