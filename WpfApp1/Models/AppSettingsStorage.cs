using Microsoft.Win32;
using Newtonsoft.Json;

namespace MTGApro.Models
{
    public class AppSettingsStorage
    {
        public bool Minimized { get; set; }

        public int Upl { get; set; }

        public int Icon { get; set; }

        public string Path { get; set; }

        public AppSettingsStorage(bool min = false, int up = 0, int ic = 0, string pa = @"", string df = @"", string df_am = @"", string df_pm = @"")
        {
            Minimized = min;
            Upl = up;
            Icon = ic;
            Path = pa;
        }


        public static AppSettingsStorage Load()
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\MTGAProtracker"))
            {
                var serializedSettings = registryKey.GetValue("appsettings").ToString();
                var settings = JsonConvert.DeserializeObject<AppSettingsStorage>(serializedSettings);

                return settings;
            }
        }

        public void Save()
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\MTGAProtracker"))
            {
                var serialized = JsonConvert.SerializeObject(this);
                registryKey.SetValue("appsettings", serialized);
            }
        }
    }
}