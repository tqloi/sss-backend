namespace SSS.Application.Features.StudySessions.Common
{
    /// <summary>
    /// Shared helper utilities for StudySessions features.
    /// </summary>
    internal static class StudySessionHelpers
    {
        /// <summary>
        /// Generates a 24-character hex string similar to MongoDB ObjectId format.
        /// </summary>
        public static string GenerateSessionId()
        {
            var timestamp = BitConverter.GetBytes((int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
            if (BitConverter.IsLittleEndian) Array.Reverse(timestamp);
            var random = new byte[8];
            Random.Shared.NextBytes(random);
            var bytes = new byte[12];
            Array.Copy(timestamp, 0, bytes, 0, 4);
            Array.Copy(random, 0, bytes, 4, 8);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
