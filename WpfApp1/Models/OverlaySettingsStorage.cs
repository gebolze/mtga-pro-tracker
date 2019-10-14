using Microsoft.Win32;
using Newtonsoft.Json;

namespace MTGApro.Models
{
    public class OverlaySettingsStorage
    {
        public int Leftdigit { get; set; }
        public int Rightdigit { get; set; }
        public int Leftdigitdraft { get; set; }
        public int Rightdigitdraft { get; set; }
        public int Font { get; set; }
        public bool Streamer { get; set; }
        public bool Decklist { get; set; }
        public bool Autoswitch { get; set; }
        public bool Showcard { get; set; }
        public bool Showtimers { get; set; }
        public bool Hotkeys { get; set; }

        public OverlaySettingsStorage(int leftdigit = 0, int rightdigit = 2, int leftdigitdraft = 0, int rightdigitdraft = 1, bool streamer = false, bool decklist = true, bool autoswitch = true, bool showcard = true, bool showtimers = true, int font = 0, bool hotkeys = true)
        {
            Leftdigit = leftdigit;
            Rightdigit = rightdigit;
            Leftdigitdraft = leftdigitdraft;
            Rightdigitdraft = rightdigitdraft;
            Streamer = streamer;
            Decklist = decklist;
            Autoswitch = autoswitch;
            Showcard = showcard;
            Showtimers = showtimers;
            Font = font;
            Hotkeys = hotkeys;
        }

        public static OverlaySettingsStorage Load()
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\MTGAProtracker"))
            {
                var serializedSettings = registryKey.GetValue("ovlsettings").ToString();
                var settings = JsonConvert.DeserializeObject<OverlaySettingsStorage>(serializedSettings);

                return settings;
            }
        }

        public void Save()
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\MTGAProtracker"))
            {
                var serialized = JsonConvert.SerializeObject(this);
                registryKey.SetValue("ovlsettings", serialized);
            }
        }
    }
}