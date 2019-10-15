using System.Collections.Generic;

namespace MTGApro.API.Models
{
    public class Battle
    {
        public Dictionary<int, int> Udecklive { get; set; }
        public Dictionary<int, int> Edeck { get; set; }
        public Dictionary<int, int> Deckstruct { get; set; }
        public string Udeck_fp { get; set; }
        public string Edeckname { get; set; }
        public string Humanname { get; set; }

        public Battle(Dictionary<int, int> deckstruct, string udeck_fp, string humanname = @"", string edeckname = @"")
        {
            Udecklive = new Dictionary<int, int>();
            Edeck = new Dictionary<int, int>();
            Edeckname = edeckname;
            Deckstruct = deckstruct;
            Udeck_fp = udeck_fp;
            Humanname = humanname;
        }
    }
}