using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using MTGApro.API;
using MTGApro.API.Models;

namespace MTGApro
{
    /// <summary>
    /// Логика взаимодействия для NotificationsWindow.xaml
    /// </summary>
    /// 

    public partial class NotificationsWindow : Window
    {
        public NotificationsWindow()
        {
            Notification[] notifications = ApiClient.GetNotifications(MainWindow.Usertoken, MainWindow.ouruid);

            string output = @"";
            for (int i = 0; i <= (notifications.Length - 1); i++)
            {
                DateTime date = MainWindow.tmstmptodate(notifications[i].Date);
                string txt = notifications[i].Txt;
                output += @"------------------------"
                          + Environment.NewLine + date + @":" + Environment.NewLine
                          + txt + Environment.NewLine
                          + @"------------------------";
            }

            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                OutputText.Text = output;
            }));

            InitializeComponent();
        }
    }
}
