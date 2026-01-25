namespace SSS.Web.Common.Response
{
    public class GenericResponse<T>
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
