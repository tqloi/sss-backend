namespace LecX.Application.Common.Dtos
{
    public abstract record GenericResponseRecord<T>(
        bool Success,
        string Message,
        T? Data = default
    );
}
