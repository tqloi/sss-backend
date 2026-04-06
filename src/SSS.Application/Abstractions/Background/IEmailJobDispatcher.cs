namespace SSS.Application.Abstractions.Background
{
    /// <summary>
    /// Dispatches email-sending jobs to the background queue.
    /// </summary>
    public interface IEmailJobDispatcher
    {
        void DispatchSendEmail(string to, string subject, string body);
    }
}