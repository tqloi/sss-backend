using MediatR;
using System.Text.Json.Serialization;
using SSS.Application.Features.Auth.Common;

namespace SSS.Application.Features.Auth.Refresh
{
    public sealed class RefreshCommand : IRequest<AuthResult>
    {
        [JsonIgnore]                      // lấy từ cookie ở Endpoint
        public string? RefreshTokenPlain { get; set; }

        [JsonIgnore]                      // gán ở Endpoint
        public string? RequestIp { get; set; }

        // nếu FE có truyền returnUrl (query/body), Endpoint gán vào đây
        public string? ReturnUrl { get; set; }
    }
}
