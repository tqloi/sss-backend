namespace SSS.Domain.Constants
{
    public static class CacheConstants
    {
        public const string AdminDashboardOverviewKey = "admin:dashboard:overview";

        // Thời gian cho các dữ liệu thay đổi liên tục
        public const int DefaultExpirationInMinutes = 30;

        // Thời gian cho dữ liệu tĩnh như Roadmap, Subject
        public const int StaticDataExpirationInMinutes = 7 * 24 * 60; // 7 days

        public static TimeSpan DefaultExpiration => TimeSpan.FromMinutes(DefaultExpirationInMinutes);
        public static TimeSpan StaticDataExpiration => TimeSpan.FromMinutes(StaticDataExpirationInMinutes);
    }
}
