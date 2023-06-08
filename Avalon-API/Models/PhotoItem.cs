namespace Avalon_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class PhotoItem
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public string? PublicationDate { get; set; }
    public string? Hashtags { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}