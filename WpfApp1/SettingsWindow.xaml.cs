using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MTGApro.API;
using MTGApro.Models;

namespace MTGApro
{
    public partial class SettingsWindow : Window
    {
        public RegistryKey RkApp { get; private set; }
        public static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MTGAproTracker");
        public static AppSettingsStorage appsettings = new AppSettingsStorage();
        public static OverlaySettingsStorage ovlsettings = new OverlaySettingsStorage();
        public static Dictionary<string, string> Usermtgaid = new Dictionary<string, string>();
        public static Dictionary<string, string> Utokens = new Dictionary<string, string>();
        public static Dictionary<string, string> Credentials = new Dictionary<string, string>();
        public static int selid = 0;
        public static string selectedacc = "0";
        public static string filename = @"";
        public static bool dateok = false;

        public SettingsWindow()
        {
            InitializeComponent();

            RkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (RkApp.GetValue("MTGApro") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                Startupchk.IsChecked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                Startupchk.IsChecked = true;
            }

            RegistryKey RkTokens = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\MTGAProtracker");

            if (RkTokens != null)
            {
                try
                {
                    appsettings = AppSettingsStorage.Load();
                    ovlsettings = OverlaySettingsStorage.Load();
                }
                catch (Exception ee)
                {
                    //MainWindow.ErrReport(ee);
                }

                if (appsettings.Minimized)
                {
                    Minimizedchk.IsChecked = true;
                }

                Uploads.SelectedIndex = appsettings.Upl;
                Icoselector.SelectedIndex = appsettings.Icon;

                DigitsToShow_left.SelectedIndex = ovlsettings.Leftdigit;
                DigitsToShow_right.SelectedIndex = ovlsettings.Rightdigit;
                DigitsToShow_left_draft.SelectedIndex = ovlsettings.Leftdigitdraft;
                DigitsToShow_right_draft.SelectedIndex = ovlsettings.Rightdigitdraft;
                Font_selector.SelectedIndex = ovlsettings.Font;

                if (ovlsettings.Streamer == true)
                {
                    Streamer.IsChecked = true;
                }
                else
                {
                    Streamer.IsChecked = false;
                }

                if (ovlsettings.Decklist == true)
                {
                    DeckList.IsChecked = true;
                }
                else
                {
                    DeckList.IsChecked = false;
                }

                if (ovlsettings.Autoswitch == true)
                {
                    Autoswitch.IsChecked = true;
                }
                else
                {
                    Autoswitch.IsChecked = false;
                }

                if (ovlsettings.Showcard == true)
                {
                    Showcard.IsChecked = true;
                }
                else
                {
                    Showcard.IsChecked = false;
                }

                if (ovlsettings.Showtimers == true)
                {
                    Showtimers.IsChecked = true;
                }
                else
                {
                    Showtimers.IsChecked = false;
                }

                if (ovlsettings.Hotkeys == true)
                {
                    Hotkeys.IsChecked = true;
                }
                else
                {
                    Hotkeys.IsChecked = false;
                }

                Usermtgaid = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(RkTokens.GetValue("nicks").ToString());
                Utokens = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(RkTokens.GetValue("utokens").ToString());
                Credentials = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(RkTokens.GetValue("credentials").ToString());

                foreach (KeyValuePair<string, string> tkn in Usermtgaid)
                {
                    AccountSelector.Items.Add(tkn.Value);
                }

                RkTokens.Close();
            }

        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {

            if (Startupchk.IsChecked == true)
            {
                RkApp.SetValue("MTGApro", System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                RkApp.DeleteValue("MTGApro", false);
            }

            if (Minimizedchk.IsChecked == true)
            {
                appsettings.Minimized = true;
            }
            else
            {
                appsettings.Minimized = false;
            }

            appsettings.Upl = Uploads.SelectedIndex;
            appsettings.Icon = Icoselector.SelectedIndex;
            appsettings.Path = filename;

            appsettings.Save();

            ovlsettings.Leftdigit = DigitsToShow_left.SelectedIndex;
            ovlsettings.Rightdigit = DigitsToShow_right.SelectedIndex;
            ovlsettings.Leftdigitdraft = DigitsToShow_left_draft.SelectedIndex;
            ovlsettings.Rightdigitdraft = DigitsToShow_right_draft.SelectedIndex;
            ovlsettings.Font = Font_selector.SelectedIndex;

            if (Streamer.IsChecked == true)
            {
                ovlsettings.Streamer = true;
            }
            else
            {
                ovlsettings.Streamer = false;
            }

            if (DeckList.IsChecked == true)
            {
                ovlsettings.Decklist = true;
            }
            else
            {
                ovlsettings.Decklist = false;
            }

            if (Autoswitch.IsChecked == true)
            {
                ovlsettings.Autoswitch = true;
            }
            else
            {
                ovlsettings.Autoswitch = false;
            }

            if (Showcard.IsChecked == true)
            {
                ovlsettings.Showcard = true;
            }
            else
            {
                ovlsettings.Showcard = false;
            }

            if (Showtimers.IsChecked == true)
            {
                ovlsettings.Showtimers = true;
            }
            else
            {
                ovlsettings.Showtimers = false;
            }

            if (Hotkeys.IsChecked == true)
            {
                ovlsettings.Hotkeys = true;
            }
            else
            {
                ovlsettings.Hotkeys = false;
            }

            ovlsettings.Save();

            MainWindow.ni.Visible = false;
            MainWindow.ni.Dispose();
            Process.Start(Application.ResourceAssembly.Location, "restarting");
            Environment.Exit(0);
        }

        private void LocateGameLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".txt",
                    FileName = "output_log.txt",
                    Filter = "MTGA Logs|output_log.txt"
                };

                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).ToString();
                }

                path += @"\AppData\LocalLow\Wizards Of The Coast\MTGA\";
                if (Directory.Exists(path))
                {
                    dlg.InitialDirectory = path;
                }

                bool? result = dlg.ShowDialog();


                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    filename = dlg.FileName;
                    LocateBtn.Content = @"Log Located!";
                }
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken);
            }
        }

        private void AccountSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selid = AccountSelector.SelectedIndex;
            string nick = AccountSelector.SelectedItem.ToString();
            /* Usermtgaid = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(RkTokens.GetValue("nicks").ToString());
             Utokens = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(RkTokens.GetValue("utokens").ToString());*/

            foreach (KeyValuePair<string, string> tkn in Usermtgaid)
            {
                if (tkn.Value == nick)
                {
                    Ingame_nick.Content = nick;
                    Acc_token.Content = Utokens[tkn.Key].ToString();
                    Ingame_nick_label.Text = @"MTGA Nick";
                    Token_label.Text = @"MTGA Pro Token";
                    selectedacc = tkn.Key;
                }
            }
        }

        private void RemoveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();

            if (mainWindow.TokenInput.Text == Utokens[selectedacc])
            {
                MessageBox.Show("You can't remove your current account! Use MANUAL RESET instead!");
            }
            else
            {
                Usermtgaid.Remove(selectedacc);
                Utokens.Remove(selectedacc);
                Credentials.Remove(selectedacc);

                string ids = Newtonsoft.Json.JsonConvert.SerializeObject(Credentials);
                string nicks = Newtonsoft.Json.JsonConvert.SerializeObject(Usermtgaid);
                string tokens = Newtonsoft.Json.JsonConvert.SerializeObject(Utokens);

                RegistryKey RkUpdate = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\MTGAProtracker", true);

                for (int i = 0; i < 11; i++)
                {
                    RkUpdate.SetValue("hash_" + i + "_" + selectedacc, @"");
                }

                RkUpdate.SetValue("credentials", ids);
                RkUpdate.SetValue("utokens", tokens);
                RkUpdate.SetValue("nicks", nicks);
                RkUpdate.Close();

                AccountSelector.Items.Remove(selid);
                Ingame_nick.Content = @"";
                Acc_token.Content = @"";
                Ingame_nick_label.Text = @"";
                Token_label.Text = @"";
                selectedacc = "0";
            }
        }
    }
}
