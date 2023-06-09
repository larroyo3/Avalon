using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;

namespace Avalon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserContext _context;

    public UsersController(UserContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        return await _context.Users
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(long id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return ItemToDTO(user);
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(long id, UserDTO userDTO)
    {
        if (id != userDTO.Id)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Name = userDTO.Name;
        user.ProfilePhoto = userDTO.ProfilePhoto;
        user.Package = userDTO.Package;
        user.Password = userDTO.Password;
        user.RemainingPhoto = userDTO.RemainingPhoto;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<UserDTO>> Postuser(UserDTO userDTO)
    {
        var user = new User
        {
            Name = userDTO.Name,
            ProfilePhoto = userDTO.ProfilePhoto,
            Package = userDTO.Package,
            RemainingPhoto = userDTO.RemainingPhoto,
            Password = userDTO.Password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            ItemToDTO(user));
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
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

    //LOGIN
    [HttpPost("/login")]
    public async Task<ActionResult<UserDTO>> LoginUser(Auth auth)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == auth.Name);
        if (user == null)
        {
            return NotFound();
        }

        if (user.Password != auth.Password)
        {
            return BadRequest();
        }

        return ItemToDTO(user);
    }

    private bool UserExists(long id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

    private static UserDTO ItemToDTO(User user) =>
       new UserDTO
       {
           Id = user.Id,
           Name = user.Name,
           ProfilePhoto = user.ProfilePhoto,
           Package = user.Package,
           RemainingPhoto = user.RemainingPhoto,
           Password = user.Password
       };
}