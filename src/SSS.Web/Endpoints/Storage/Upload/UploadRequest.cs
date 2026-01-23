namespace SSS.WebApi.Endpoints.Storage.Upload
{
    public sealed class UploadRequest
    {
        public IFormFile File { get; set; } = default!;
        public string? Prefix { get; set; } // ví dụ: "public/user-avatars"
    }
}