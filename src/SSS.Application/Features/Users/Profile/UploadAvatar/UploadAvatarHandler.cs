using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Application.Abstractions.External.Storage.Gcs;
using SSS.Domain.Entities.Identity;


namespace SSS.Application.Features.Users.Profile.UploadAvatar;

public class UploadAvatarHandler : IRequestHandler<UploadAvatarCommand, UploadAvatarResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IGcsStorageService _storageService;

    public UploadAvatarHandler(UserManager<User> userManager, IGcsStorageService storageService)
    {
        _userManager = userManager;
        _storageService = storageService;
    }

    public async Task<UploadAvatarResult> Handle(UploadAvatarCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new KeyNotFoundException("User not found");

        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(request.ContentType.ToLowerInvariant()))
            throw new ArgumentException("Invalid file type. Only JPEG, PNG, GIF, and WebP are allowed.");

        // Generate unique filename
        var extension = Path.GetExtension(request.FileName);
        var uniqueFileName = $"avatars/{user.Id}/{Guid.NewGuid()}{extension}";

        // Delete old avatar if exists
        if (!string.IsNullOrEmpty(user.AvatarUrl))
        {
            await _storageService.DeleteAsync(user.AvatarUrl, ct);
        }

        // Upload new avatar
        var uploadedFileName = await _storageService.UploadAsync(
            request.FileStream,
            uniqueFileName,
            request.ContentType,
            ct);

        var avatarUrl = _storageService.GetPublicUrl(uploadedFileName);

        // Update user avatar URL
        user.AvatarUrl = avatarUrl;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return new UploadAvatarResult
        {
            AvatarUrl = avatarUrl
        };
    }
}
