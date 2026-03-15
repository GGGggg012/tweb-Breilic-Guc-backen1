namespace eAviaSales.DataAccess
{
    public static class DbSession
    {
        public static string ConnectionString { get; set; } = "Server=localhost;Database=eAviaSales;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
