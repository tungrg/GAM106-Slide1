namespace WebApplication1.Models;

public class CreateUserRequest
{
    public required string Username { get; set; }
    public int RegionId { get; set; }
    public int RoleId { get; set; }
    public string? LinkAvatar { get; set; }
    public int Otp { get; set; }
}
