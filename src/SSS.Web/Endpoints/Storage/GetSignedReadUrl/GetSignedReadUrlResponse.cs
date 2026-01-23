namespace SSS.WebApi.Endpoints.Storage.GetSignedReadUrl
{
    public sealed class GetSignedReadUrlResponse
    {
        public bool Success { get; set; }
        public string Url { get; set; } = string.Empty;
    }

}