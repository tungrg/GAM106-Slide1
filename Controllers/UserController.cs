namespace WebApplication1.Controllers;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users
            .Include(u => u.role)
            .Include(u => u.region)
            .ToListAsync();
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.Include(u => u.role).Include(u => u.region).FirstOrDefaultAsync(u => u.userId == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(CreateUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.username == request.Username))
        {
            return BadRequest("Username already exists.");
        }

        var region = await _context.Regions.FindAsync(request.RegionId);
        var role = await _context.Roles.FindAsync(request.RoleId);

        if (region == null)
        {
            return BadRequest("Invalid Region ID");
        }

        if (role == null)
        {
            return BadRequest("Invalid Role ID");
        }

        var user = new User
        {
            username = request.Username,
            region = region,
            role = role,
            linkAvatar = request.LinkAvatar,
            otp = request.Otp
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.userId}, user);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.userId)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}