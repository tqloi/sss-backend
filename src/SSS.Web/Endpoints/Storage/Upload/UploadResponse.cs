namespace SSS.WebApi.Endpoints.Storage.Upload
{
    public sealed class UploadResponse
    {
        public bool Success { get; set; }
        public string ObjectName { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}