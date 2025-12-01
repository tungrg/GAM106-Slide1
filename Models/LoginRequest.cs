namespace WebApplication1.Models;

public class LoginRequest
{
    public required string Username { get; set; }
    public int Otp { get; set; }
}
