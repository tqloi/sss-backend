namespace SSS.Infrastructure.External.Storage.Gcs
{
    public class GcsStorageOptions
    {
        public string Bucket { get; set; } = string.Empty;
        public string? KeyPath { get; set; }
    }
}