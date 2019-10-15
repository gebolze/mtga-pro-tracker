﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MTGApro.API;

namespace MTGApro
{
    /// <summary>
    /// Логика взаимодействия для ToolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        public ToolsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string command = (sender as Button).Name.ToString();
                if (command == @"decks")
                {
                    Uri link = new Uri(@"https://mtgarena.pro/decks/");
                    Process.Start(new ProcessStartInfo(link.AbsoluteUri));
                }
                else if (command == @"mydecks")
                {
                    Uri link = new Uri(@"https://mtgarena.pro/decks/?my");
                    Process.Start(new ProcessStartInfo(link.AbsoluteUri));
                }
                else if (command == @"mycol")
                {
                    Uri link = new Uri(@"https://mtgarena.pro/collection/");
                    Process.Start(new ProcessStartInfo(link.AbsoluteUri));
                }
                else if (command == @"myprogr")
                {
                    Uri link = new Uri(@"https://mtgarena.pro/progress/");
                    Process.Start(new ProcessStartInfo(link.AbsoluteUri));
                }
                else if (command == @"deckbuilder")
                {
                    Uri link = new Uri(@"https://mtgarena.pro/deckbuilder/");
                    Process.Start(new ProcessStartInfo(link.AbsoluteUri));
                }
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken);
            }
        }
    }
}
