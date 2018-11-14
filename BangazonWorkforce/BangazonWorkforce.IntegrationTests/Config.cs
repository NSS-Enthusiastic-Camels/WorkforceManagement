namespace BangazonWorkforce.IntegrationTests
{
    public static class Config
    {
        public static string ConnectionSring
        {
            get
            {
                return "Server=STEVE-PC\\SQLEXPRESS;Initial Catalog=BangazonAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            }
        }
    }
}
