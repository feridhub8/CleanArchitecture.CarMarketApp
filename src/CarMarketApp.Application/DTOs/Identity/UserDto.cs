namespace CarMarketApp.Application.DTOs.Identity;

public sealed record UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public IReadOnlyList<string> Roles { get; set; } = new List<string>();
}
