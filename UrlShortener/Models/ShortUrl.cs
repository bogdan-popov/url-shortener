namespace UrlShortener.Models
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }
        public User User { get; set; } 
    }
}
