namespace SSS.Infrastructure.External.AI.Gemini
{
    public class GeminiOptions
    {
        public string ApiKey { get; set; } = default!;
        public string Model { get; set; } = "gemini-1.5-flash";
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/";
    }
}
