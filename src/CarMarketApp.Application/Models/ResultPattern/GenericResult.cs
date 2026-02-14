namespace CarMarketApp.Application.Models.ResultPattern;

public class Result<T> : Result
{
    public T? Data { get; init; }

    protected Result(bool success, T? data = default, string? message = null, Dictionary<string, string[]>? errors = null)
        : base(success, message, errors)
    {
        Data = data;
    }

    public static Result<T> Ok(T data, string? message = null) => new(true, data, message);
    public new static Result<T> Fail(string? message = null, Dictionary<string, string[]>? errors = null) => new(false, default, message, errors);
}
