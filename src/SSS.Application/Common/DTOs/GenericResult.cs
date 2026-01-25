namespace SSS.Application.Common.DTOs
{
    public class GenericResult<T>
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
