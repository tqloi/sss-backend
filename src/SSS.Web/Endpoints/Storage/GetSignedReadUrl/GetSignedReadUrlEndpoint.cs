using FastEndpoints;
using SSS.Application.Abstractions.External.Storage.Gcs;

namespace SSS.WebApi.Endpoints.Storage.GetSignedReadUrl
{
    public sealed class GetSignedReadUrlEndpoint(IGcsStorageService storage)
        : Endpoint<GetSignedReadUrlRequest, GetSignedReadUrlResponse>
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

        public override Task HandleAsync(GetSignedReadUrlRequest req, CancellationToken ct)
        {
            var url = storage.GetSignedReadUrl(req.ObjectName, TimeSpan.FromSeconds(req.TtlSeconds));
            return SendOkAsync(new GetSignedReadUrlResponse { Success = true, Url = url }, ct);
        }
    }
}
