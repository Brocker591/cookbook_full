namespace Cookbook.App.Dtos;

public class UserDto
{
    public int UserId { get; set; }
    public string access_token { get; set; } = default!;
    public string token_type { get; set; } = default!;
    public int expiresIn { get; set; }
}
