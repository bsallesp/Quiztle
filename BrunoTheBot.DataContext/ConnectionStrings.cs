namespace BrunoTheBot.DataContext
{
    public static class ConnectionStrings
    {
        public static string DevelopmentConnectionString
            = "Host=localhost;Database=BrunoTheBotDB;port=5432;Username=brunothebot;Password=@pyramid2050!";
        public static string ProductionConnectionString
            = "Host=localhost;Database=BrunoTheBotDB;Username=brunothebot;Password=my_pw";
    }
}
