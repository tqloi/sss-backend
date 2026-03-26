using FastEndpoints;
using SSS.Application.Abstractions.External.Storage.Gcs;

namespace SSS.WebApi.Endpoints.Storage.GetSignedReadUrl
{
    public sealed class GetSignedReadUrlEndpoint(IGcsStorageService storage)
        : EndpointWithoutRequest<GetSignedReadUrlResponse>
    {
        public override void Configure()
        {
            Get("/api/storage/signed-read");
            // AllowAnonymous(); // hoặc Authorize();
            Summary(s =>
            {
                s.Summary = "Lấy URL đọc có chữ ký (GET)";
                s.Description = "Trả về signed URL cho phép client tải xuống trong TTL chỉ định.";
            });
        }

        public override Task HandleAsync(CancellationToken ct)
        {
            var objectName = Query<string>("objectName");
            var ttlSeconds = Query<int?>("ttlSeconds") ?? 300;

            if (string.IsNullOrWhiteSpace(objectName) || ttlSeconds <= 0 || ttlSeconds > 3600)
            {
                AddError(r => r, "objectName is required and ttlSeconds must be between 1 and 3600.");
                return SendErrorsAsync(cancellation: ct);
            }

            var url = storage.GetSignedReadUrl(objectName, TimeSpan.FromSeconds(ttlSeconds));
            return SendOkAsync(new GetSignedReadUrlResponse { Success = true, Url = url }, ct);
        }
    }
}
