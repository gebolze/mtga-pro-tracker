using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MTGApro
{
    /// <summary>
    /// Логика взаимодействия для ErrorReportWindow.xaml
    /// </summary>
    /// 

    public partial class ErrorReportWindow : Window
    {

        public class Notifi
        {
            public double Date { get; set; }
            public string Txt { get; set; }

            public Notifi(int date, string txt)
            {
                Date = date;
                Txt = txt;
            }
        }

        public ErrorReportWindow()
        {
           OutputText.Text = MainWindow.errreport;


            InitializeComponent();
        }
    }
}
