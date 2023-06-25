using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;
using Avalon_API.DAL;

namespace Avalon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILogger _logger;
    private IUserRepository _userRepository;

    public UsersController(UserContext context, ILogger<UsersController> logger)
    {
        _userRepository = new UserRepository(context);
        _logger = logger;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
    {
        _logger.LogInformation("GET users at {DT} by user_id: 1", 
            DateTime.UtcNow.ToLongTimeString());

        var users = await _userRepository.GetUsersAsync();
        return Ok(users);
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(long id)
    {
        var user = await _userRepository.GetUserByIDAsync(id);

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

        var user = await _userRepository.GetUserByIDAsync(id);
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
            await _userRepository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException) when (!_userRepository.UserExists(id))
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

        _userRepository.InsertUser(user);
        await _userRepository.SaveAsync();

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
        var user = await _userRepository.GetUserByIDAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _logger.LogInformation("POST user id: '{ID}' at {DT} by user_id : 1", 
            id, DateTime.UtcNow.ToLongTimeString());

        _userRepository.DeleteUser(user);
        await _userRepository.SaveAsync();

        return NoContent();
    }

    //LOGIN
    [HttpPost("/login")]
    public async Task<ActionResult<UserDTO>> LoginUser(Auth auth)
    {
        var user = await _userRepository.Login(auth);
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