namespace SSS.WebApi.Endpoints.Storage.GetSignedWriteUrl
{
    public sealed class GetSignedWriteUrlResponse
    {
        public bool Success { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}