using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;

namespace Avalon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserContext _context;
    private readonly ILogger _logger;

    public UsersController(UserContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        _logger.LogInformation("GET users at {DT} by user_id: 1", 
            DateTime.UtcNow.ToLongTimeString());

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

        _logger.LogInformation("GET user by Id at {DT} by user_id: 1", 
            DateTime.UtcNow.ToLongTimeString());

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

        _logger.LogInformation("PUT user name: '{NAME}' at {DT} by user_id : 1", 
            userDTO.Name, DateTime.UtcNow.ToLongTimeString());

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

        _logger.LogInformation("POST user name: '{NAME}' at {DT} by user_id : 1", 
            userDTO.Name, DateTime.UtcNow.ToLongTimeString());

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

        _logger.LogInformation("POST user id: '{ID}' at {DT} by user_id : 1", 
            id, DateTime.UtcNow.ToLongTimeString());

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

        _logger.LogInformation("POST user name: '{NAME}' at {DT} by user_id : 1", 
            auth.Name, DateTime.UtcNow.ToLongTimeString());

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