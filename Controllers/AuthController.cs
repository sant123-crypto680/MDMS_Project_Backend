using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MDMS_PRoject_Backend.Data;
using MDMS_PRoject_Backend.Models;
 
namespace MDMS_PRoject_Backend.Controllers;
 
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
 
    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }
 
    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(
        [FromBody] LoginRequest request)
    {
        // STEP 1: Check whether email exists
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == request.Email.ToLower());
 
        if (user == null)
        {
            return NotFound(new
            {
                message = "Email not found"
            });
        }
 
        // STEP 2: Check whether role matches
        if (user.Role.ToLower() != request.Role.ToLower())
        {
            return BadRequest(new
            {
                message = "Role does not match"
            });
        }
 
        // STEP 3: Return user details
        return Ok(user);
    }
}
 
// Request model for login