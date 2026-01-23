using FastEndpoints;
using SSS.Application.Abstractions.External.Storage.Gcs;

namespace SSS.WebApi.Endpoints.Storage.GetSignedResumableUrl
{
    public sealed class GetSignedResumableUrlEndpoint(IGcsStorageService storage)
      : Endpoint<GetSignedResumableUrlRequest, GetSignedResumableUrlResponse>
    {
        public override void Configure()
        {
            Get("/api/storage/signed-resumable");
            // Authorize();
            Summary(s =>
            {
                s.Summary = "Lấy URL khởi tạo Resumable Upload (POST)";
                s.Description = "Dùng cho upload chunked; client gọi kèm header 'x-goog-resumable: start'.";
            });
        }

        public override Task HandleAsync(GetSignedResumableUrlRequest req, CancellationToken ct)
        {
            var url = storage.GetSignedResumableInitiationUrl(
                req.ObjectName,
                req.ContentType,
                TimeSpan.FromSeconds(req.TtlSeconds));

            return SendOkAsync(new GetSignedResumableUrlResponse { Success = true, Url = url }, ct);
        }
    }
}
