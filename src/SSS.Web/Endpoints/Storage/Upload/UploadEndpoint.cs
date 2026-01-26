using FastEndpoints;
using SSS.Application.Abstractions.External.Storage.Gcs;
using System.Text.RegularExpressions;

namespace SSS.WebApi.Endpoints.Storage.Upload
{
    public sealed class UploadEndpoint(IGcsStorageService storage)
       : Endpoint<UploadRequest, UploadResponse>
    {
        public override void Configure()
        {
            Post("/api/storage/upload");
            // Authorize(); // bật nếu cần
            AllowFileUploads();
            Summary(s =>
            {
                s.Summary = "Upload file lên GCS qua server";
                s.Description = "Nhận multipart file, upload bằng server và trả về objectName + public URL. Chỉ dùng khi up file nhỏ (dưới 20mb)";
            });
        }

        public override async Task HandleAsync(UploadRequest req, CancellationToken ct)
        {
            var file = req.File;
            var contentType = string.IsNullOrWhiteSpace(file.ContentType)
                ? "application/octet-stream"
                : file.ContentType;

            await using var stream = file.OpenReadStream();

            var (fileName,ext) = FileNameHelper.Normalize(file.FileName);

            //var folder = string.IsNullOrWhiteSpace(req.Prefix)
            //    ? GoogleStoragePaths.Default
            //    : req.Prefix.Trim().Trim('/');

            var folder = "default";

            // thêm Guid phía sau để tránh trùng tên
            var objectName = $"{folder}/{fileName}-{Guid.NewGuid():N}{ext}";

            var savedName = await storage.UploadAsync(stream, objectName, contentType, ct);
            var publicUrl = storage.GetPublicUrl(savedName);

            await SendOkAsync(new UploadResponse
            {
                Success = true,
                ObjectName = savedName,
                PublicUrl = publicUrl,
                FileName = fileName
            }, ct);
        }
    }

    public static class FileNameHelper
    {
        /// <summary>
        /// Chuẩn hóa tên file: bỏ ký tự đặc biệt, khoảng trắng → '-', lowercase.
        /// </summary>
        /// <param name="originalName">Tên file gốc (vd: 'Ảnh đại diện mới (1).PNG')</param>
        /// <returns>
        /// Tuple gồm:
        ///  - SafeName: tên file đã chuẩn hóa (không có extension)
        ///  - Extension: phần mở rộng (vd: '.png')
        /// </returns>
        public static (string SafeName, string Extension) Normalize(string originalName)
        {
            if (string.IsNullOrWhiteSpace(originalName))
                return ("unnamed-file", string.Empty);

            var ext = Path.GetExtension(originalName).ToLowerInvariant();
            var name = Path.GetFileNameWithoutExtension(originalName).Trim().ToLowerInvariant();

            // Thay khoảng trắng bằng '-'
            name = Regex.Replace(name, @"\s+", "-");

            // Xóa ký tự đặc biệt / unicode lạ (chỉ giữ chữ, số, '-', '_')
            name = Regex.Replace(name, @"[^a-z0-9\-_]", "");

            // Giới hạn độ dài để tránh vượt giới hạn GCS (tùy chọn)
            if (name.Length > 100)
                name = name[..100];

            if (string.IsNullOrWhiteSpace(name))
                name = "unnamed-file";

            return (name, ext);
        }
    }
}
