using FastEndpoints;
using SSS.Application.Abstractions.External.Storage.Gcs;

namespace SSS.WebApi.Endpoints.Storage.GetSignedWriteUrl
{
    public sealed class GetSignedWriteUrlEndpoint(IGcsStorageService storage)
       : EndpointWithoutRequest<GetSignedWriteUrlResponse>
    {
        public override void Configure()
        {
            Get("/api/storage/signed-write");
            // Authorize();
            Description(d => d.WithTags("Storage"));
            Summary(s =>
            {
                s.Summary = "Lấy URL ghi có chữ ký (PUT)";
                s.Description = "Client upload trực tiếp lên GCS qua PUT, phải gửi đúng Content-Type khi PUT.";
            });
        }

        public override Task HandleAsync(CancellationToken ct)
        {
            var objectName = Query<string>("objectName");
            var contentType = Query<string>("contentType") ?? "application/octet-stream";
            var ttlSeconds = Query<int?>("ttlSeconds") ?? 300;

            if (string.IsNullOrWhiteSpace(objectName) || string.IsNullOrWhiteSpace(contentType) || ttlSeconds <= 0 || ttlSeconds > 3600)
            {
                AddError(r => r, "objectName and contentType are required; ttlSeconds must be between 1 and 3600.");
                return SendErrorsAsync(cancellation: ct);
            }

            var url = storage.GetSignedWriteUrl(
                objectName,
                contentType,
                TimeSpan.FromSeconds(ttlSeconds));

            return SendOkAsync(new GetSignedWriteUrlResponse { Success = true, Url = url }, ct);
        }
    }
}
