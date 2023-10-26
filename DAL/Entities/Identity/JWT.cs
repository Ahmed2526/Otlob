namespace DAL.Entities.Identity
{
    public class JWT
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double TokenDurationInDays { get; set; }
        public double RefreshTokenDurationInDays { get; set; }
        public string Key { get; set; }
    }
}
