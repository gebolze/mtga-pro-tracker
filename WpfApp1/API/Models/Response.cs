namespace MTGApro.API.Models
{
    public class Response
    {
        public string Status { get; set; }
        public string Data { get; set; }
        public int Chunk { get; set; }
        public int Timer { get; set; }
        public string Package { get; set; }

        public Response(string status, string data, int chunk = 50, int timer = 0, string package = @"")
        {
            Status = status;
            Data = data;
            Chunk = chunk;
            Timer = timer;
            Package = package;
        }
    }
}