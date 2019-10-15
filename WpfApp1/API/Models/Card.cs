namespace MTGApro.API.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int Doublelink { get; set; }
        public string Cardid { get; set; }
        public int Multiverseid { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Kw { get; set; }
        public string Flavor { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public int Expansion { get; set; }
        public int Rarity { get; set; }
        public string Mana { get; set; }
        public int Convmana { get; set; }
        public int Loyalty { get; set; }
        public int Type { get; set; }
        public int Subtype { get; set; }
        public string Txttype { get; set; }
        public string Pict { get; set; }
        public string Date_in { get; set; }
        public string Colorindicator { get; set; }
        public int Mtga_id { get; set; }
        public int Is_collectible { get; set; }
        public int Reprint { get; set; }
        public int Supercls { get; set; }
        public int Draftrate { get; set; }
        public int Drafteval { get; set; }
        public int Is_land { get; set; }
        public string Colorarr { get; set; }
        public int Currentstandard { get; set; }
        public string Art { get; set; }
    }
}