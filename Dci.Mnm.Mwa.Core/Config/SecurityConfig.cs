namespace Dci.Mnm.Mwa.Core
{
    public class SecurityConfig
    {
        public TokenConfig Token { get; set; } = new TokenConfig();
        public SecurityDataConfig Data { get; set; } = new SecurityDataConfig();
        public CorsConfig Cors { get; set; } = new CorsConfig();
    }
}









