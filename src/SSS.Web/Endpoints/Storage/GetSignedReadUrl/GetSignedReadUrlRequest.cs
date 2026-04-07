namespace SSS.WebApi.Endpoints.Storage.GetSignedReadUrl
{
    public sealed class GetSignedReadUrlRequest
    {
        public string ObjectName { get; set; } = string.Empty;
        public int TtlSeconds { get; set; } = 300; // mặc định 5 phút
    }
}