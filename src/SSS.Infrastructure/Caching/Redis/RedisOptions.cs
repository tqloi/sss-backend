namespace SSS.Infrastructure.Caching.Redis
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Ssl { get; set; } = true;
    }
}
