namespace Avalon_API.Models;

public class PhotoItemDTO
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public string? PublicationDate { get; set; }
    public string? Hashtags { get; set; }
    public string? Description { get; set; }
    public string? ImageData { get; set; }
}