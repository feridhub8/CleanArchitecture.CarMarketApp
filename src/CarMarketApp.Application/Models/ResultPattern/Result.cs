namespace CarMarketApp.Application.Models.ResultPattern;

public class Result
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public Dictionary<string, string[]>? Errors { get; init; }

    protected Result(bool success, string? message = null, Dictionary<string, string[]>? errors = null)
    {
        Success = success;
        Message = message;
        Errors = errors;
    }

    public static Result Ok(string? message = null) => new(true, message);
    public static Result Fail(string? message = null, Dictionary<string, string[]>? errors = null) => new(false, message, errors);
}
