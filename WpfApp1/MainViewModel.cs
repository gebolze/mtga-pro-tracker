using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using MTGApro.API;
using Stylet;

namespace MTGApro
{
    public class MainViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        public MainViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        // TODO: We should never access the view directly, all interaction with the view should be done using bindings
        private MainWindow MainWindow
        {
            get { return (MainWindow) View; }
        }

        public void ShowSettings()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                _windowManager.ShowWindow(new SettingsViewModel());
            }
            catch (Exception ex)
            {
                ApiClient.ErrorReport(ex, MainWindow.Usertoken, 2149);
            }
        }

        public void ShowTools()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                _windowManager.ShowWindow(new ToolsViewModel());
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken, 2207);
            }
        }

        public void ManualResync()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                MainWindow.juststarted = true;
                MainWindow.hashes = new string[MainWindow.indicators.Length];
                MainWindow.loglen = 0;
                MainWindow.parsedtill = 0;
                MainWindow.manualresync = true;
                MainWindow.Showmsg(Colors.Green, @"Launching re-sync...", @"", false, @"icon" + MainWindow.appsettings.Icon.ToString());
            }
            catch (Exception)
            {

            }
        }

        public void FullReset()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MTGAproTracker");
                try
                {
                    //throw new Exception();
                    DirectoryInfo dir = new DirectoryInfo(path);

                    foreach (FileInfo file in dir.EnumerateFiles("*.mtg"))
                    {
                        file.Delete();
                    }
                }
                catch (Exception ee)
                {
                    ApiClient.ErrorReport(ee, MainWindow.Usertoken, 2170);
                }
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\\MTGAProtracker");
                MainWindow.ni.Visible = false;
                MainWindow.ni.Dispose();
                Process.Start(Application.ResourceAssembly.Location, "restarting");
                Environment.Exit(0);
            }
            catch (Exception)
            {
                MessageBox.Show("There is no app data stored, so no need to do reset!");
            }
        }

        public void StopTracker()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                MainWindow.ni.Visible = false;
                MainWindow.ni.Dispose();
                Environment.Exit(0);
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken, 2194);
            }
        }

        public void ToggleIngameOverlay()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                if (!MainWindow.overlayactive)
                {
                    MainWindow.overlayactive = true;
                    MainWindow.ovactbut.Text = "Disable In-game overlay";
                }
                else
                {
                    MainWindow.overlayactive = false;
                    MainWindow.ovactbut.Text = "Activate In-game overlay";
                    if (MainWindow.matchOverlayWindow.IsVisible == true)
                    {
                        MainWindow.matchOverlayWindow.Hide();
                    }
                }
                MainWindow.SetAppData();
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken, 2234);
            }
        }

        public void UpdateTracker()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                string curpath = Directory.GetCurrentDirectory();
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MTGAproTracker");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadFileAsync(new Uri(@"https://teslegends.pro/mtg/tracker/latest.zip"), path + @"\update.zip");
                    myWebClient.DownloadFileCompleted += MyWebClient_DownloadFileCompleted;
                    MainWindow.Updater.Content = "Loading...";
                }
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken, 2260);
            }
        }

        public void ShowMessenger()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            try
            {
                NotificationsWindow notificationsWindow = new NotificationsWindow();
                MainWindow.Messenger.Visibility = Visibility.Hidden;
                notificationsWindow.Show();
            }
            catch (Exception ee)
            {
                ApiClient.ErrorReport(ee, MainWindow.Usertoken, 2328);
            }
        }

        private void ScanOldLogs()
        {
            // TODO: We should never access the view directly, all interaction with the view should be done using bindings

            OpenFileDialog dlg = new OpenFileDialog {DefaultExt = ".htm"};
            string path = ProgramFilesx86();

            path += @"\Wizards of the Coast\MTGA\MTGA_Data\Logs\Logs";
            if (Directory.Exists(path))
            {
                dlg.InitialDirectory = path;
            }

            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                MainWindow.tmplog = dlg.FileName;
                MainWindow.juststarted = true;
                MainWindow.hashes = new string[MainWindow.indicators.Length];
                MainWindow.loglen = 0;
                MainWindow.parsedtill = 0;
                MainWindow.manualresync = true;
                MainWindow.Showmsg(Colors.Green, @"Opening the old log...", @"", false, @"icon" + MainWindow.appsettings.Icon.ToString());
            }
        }

        private void MyWebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string curpath = Directory.GetCurrentDirectory();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"MTGAproTracker");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (Directory.Exists(path + @"\update"))
            {
                Directory.Delete(path + @"\update", true);
            }

            ZipFile.ExtractToDirectory(path + @"\update.zip", path + @"\update\");
            Process.Start(path + @"\update\MTGApro_Installer.msi");
            Environment.Exit(0);
        }

        private static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }
}