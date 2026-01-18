namespace SSS.WebApi.Endpoints.Auth.Common
{
    public static class AuthHelper
    {
        public const string RefreshCookieName = "refreshToken";

        public static void AppendRefreshCookie(HttpContext ctx, string refreshPlain, DateTime refreshExpiresUtc, bool isDev)
        {
            ctx.Response.Cookies.Append(RefreshCookieName, refreshPlain, new CookieOptions
            {
                HttpOnly = true,
                Secure = !isDev,
                SameSite = SameSiteMode.Strict,
                Expires = refreshExpiresUtc
            });
        }
        public static void DeleteRefresh(HttpContext ctx)
        => ctx.Response.Cookies.Delete(RefreshCookieName);
    }
}
