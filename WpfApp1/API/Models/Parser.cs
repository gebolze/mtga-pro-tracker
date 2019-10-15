namespace MTGApro.API.Models
{
    public class Parser
    {
        public string Indicators { get; set; }
        public string Loginput { get; set; }
        public bool Send { get; set; }
        public bool Needrunning { get; set; }
        public bool Addup { get; set; }
        public string Stopper { get; set; }
        public string Needtohave { get; set; }
        public string Ignore { get; set; }

        public Parser(string indocators, bool send = true, bool needrunning = false, bool addup = false, string stopper = @"(Filename:", string needtohave = @"", string loginput = @"", string ignore = @"")
        {
            Send = send;
            Indicators = indocators;
            if (loginput != null)
            {
                Loginput = loginput;
            }
            else
            {
                Loginput = @"";
            }
            Needrunning = needrunning;
            Addup = addup;
            Stopper = stopper;
            Needtohave = needtohave;
            Ignore = ignore;
        }
    }
}