namespace Avalon_API.Models;

public class UserDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? ProfilePhoto { get; set; }
    public string? Package { get; set; }
    public int RemainingPhoto { get; set; }
    public string? Password { get; set; }
}