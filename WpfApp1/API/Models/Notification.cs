namespace MTGApro.API.Models
{
    public class Notification
    {
        public double Date { get; set; }
        public string Txt { get; set; }

        public Notification(int date, string txt)
        {
            Date = date;
            Txt = txt;
        }
    }
}