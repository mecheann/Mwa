namespace Dci.Mnm.Mwa.Core
{
    public class TokenConfig
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double TokenLifeTimeInMinutes { get; set; }
        public string HeaderPayloadCookieName { get; set; }
        public string SignatureCookieName { get; set; }
        public string UMARefreshSalt { get; set; }
    }
}









