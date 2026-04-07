namespace SSS.Application.Abstractions.Services
{
    public interface IModuleService
    {
        /// <summary>
        /// Đánh dấu module là completed và invalidate plan cache
        /// </summary>
        Task CompleteModuleAsync(int moduleId, CancellationToken ct);
    }
}
