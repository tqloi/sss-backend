namespace SSS.WebApi.Endpoints.Storage.GetSignedResumableUrl
{
    public sealed class GetSignedResumableUrlResponse
    {
        public bool Success { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Note { get; set; } = "Client must send header: x-goog-resumable: start";
    }
}