namespace LecX.Application.Common.Dtos
{
    public abstract class GenericResponseClass<T>
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
