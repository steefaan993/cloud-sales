namespace APIGateway
{
    public class RateLimitingConfig
    {
        public int PermitLimit { get; set; }
        public int Window { get; set; }
        public int SegmentsPerWindow { get; set; }
        public int QueueLimit { get; set; }
    }
}
