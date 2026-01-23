namespace SSS.WebApi.Endpoints.Storage.GetSignedWriteUrl
{
    public sealed class GetSignedWriteUrlRequest
    {
        public string ObjectName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/octet-stream";
        public int TtlSeconds { get; set; } = 300;
    }
}