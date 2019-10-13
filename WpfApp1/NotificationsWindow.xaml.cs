using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace MTGApro
{
    /// <summary>
    /// Логика взаимодействия для NotificationsWindow.xaml
    /// </summary>
    /// 

    public partial class NotificationsWindow : Window
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

        public NotificationsWindow()
        {
            string notif = MainWindow.MakeRequest(new Uri(@"https://remote.mtgarena.pro/donew.php"), new Dictionary<string, object> { { @"cmd", @"cm_getpush" }, { @"uid", MainWindow.ouruid }, { @"token", MainWindow.Usertoken } });
            Notifi[] notifparsed = Newtonsoft.Json.JsonConvert.DeserializeObject<Notifi[]>(notif);
            string output = @"";
            for (int i = 0; i <= (notifparsed.Length - 1); i++)
            {
                DateTime date = MainWindow.tmstmptodate(notifparsed[i].Date);
                string txt = notifparsed[i].Txt;
                output += @"------------------------" + Environment.NewLine + date.ToString() + @":" + Environment.NewLine + txt + Environment.NewLine + @"------------------------";
            }
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                OutputText.Text = output;
            }));

            InitializeComponent();
        }
    }
}
