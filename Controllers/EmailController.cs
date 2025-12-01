using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailModel emailRequest)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _emailService.SendEmailAsync(emailRequest);
            return Ok(new { message = "Email request processed successfully." });
        }
        catch (Exception ex)
        {
            // In a real application, you might want to log the error
            return StatusCode(500, new { message = "An error occurred while sending the email.", error = ex.Message });
        }
    }
}
