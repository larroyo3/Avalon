using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;

namespace Avalon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotoItemsController : ControllerBase
{
    private readonly PhotoContext _context;
    private readonly UserContext _userContext;

    public PhotoItemsController(PhotoContext context, UserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    // GET: api/PhotoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PhotoItemDTO>>> GetPhotoItems()
    {
        var photoItems = await _context.PhotoItems.ToListAsync();
        var photoItemsDTO = new List<PhotoItemDTO>();

        foreach (var photoItem in photoItems)
        {
            var dto = ItemToDTO(photoItem);

            if (!string.IsNullOrEmpty(photoItem.ImagePath))
            {
                var imagePath = Path.Combine("./images", photoItem.ImagePath);
                dto.ImageData = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(imagePath));
            }

            photoItemsDTO.Add(dto);
        }

        return photoItemsDTO;
    }

    // GET: api/PhotoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<PhotoItemDTO>> GetPhotoItem(long id)
    {
        var photoItem = await _context.PhotoItems.FindAsync(id);

        if (photoItem == null)
        {
            return NotFound();
        }

        var dto = ItemToDTO(photoItem);

        if (!string.IsNullOrEmpty(photoItem.ImagePath))
        {
            var imagePath = Path.Combine("./images", photoItem.ImagePath);
            dto.ImageData = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(imagePath));
        }

        return dto;
    }

    // PUT: api/PhotoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPhotoItem(long id, PhotoItemDTO photoDTO)
    {
        if (id != photoDTO.Id)
        {
            return BadRequest();
        }

        var photoItem = await _context.PhotoItems.FindAsync(id);
        if (photoItem == null)
        {
            return NotFound();
        }

        photoItem.Hashtags = photoDTO.Hashtags;
        photoItem.Description = photoDTO.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!PhotoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/PhotoItems
    [HttpPost]
    public async Task<ActionResult<PhotoItemDTO>> PostPhotoItem(PhotoItemDTO photoDTO)
    {
        var photoItem = new PhotoItem
        {
            AuthorId = photoDTO.AuthorId,
            PublicationDate = photoDTO.PublicationDate,
            Description = photoDTO.Description,
            Hashtags = photoDTO.Hashtags
        };

        if (!string.IsNullOrEmpty(photoDTO.ImageData))
        {
            // Convertir les données d'image Base64 en tableau d'octets
            var imageBytes = Convert.FromBase64String(photoDTO.ImageData);

            // Générer un nom de fichier unique pour l'image
            var fileName = Guid.NewGuid().ToString() + ".jpg";

            // Définir le chemin de stockage de l'image sur le serveur
            var filePath = Path.Combine("./images", fileName);

            // Enregistrer l'image sur le serveur
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            // Assigner le nom de fichier à la propriété ImagePath du modèle PhotoItem
            photoItem.ImagePath = fileName;
        }

        _context.PhotoItems.Add(photoItem);
        await _context.SaveChangesAsync();

        var authorItem = await _userContext.Users.FindAsync(photoDTO.AuthorId);
        if (authorItem == null)
        {
            return NotFound();
        }
        authorItem.RemainingPhoto = authorItem.RemainingPhoto - 1;

        try
        {
            await _userContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserExists(authorItem.Id))
        {
            return NotFound();
        }

        return CreatedAtAction(
            nameof(GetPhotoItem),
            new { id = photoItem.Id },
            ItemToDTO(photoItem));
    }

    // DELETE: api/PhotoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhotoItem(long id)
    {
        var photoItem = await _context.PhotoItems.FindAsync(id);
        if (photoItem == null)
        {
            return NotFound();
        }

        _context.PhotoItems.Remove(photoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PhotoItemExists(long id)
    {
        return _context.PhotoItems.Any(e => e.Id == id);
    }

    private bool UserExists(long id)
    {
        return _userContext.Users.Any(e => e.Id == id);
    }

    private static PhotoItemDTO ItemToDTO(PhotoItem photoItem)
    {
        var photoItemDTO = new PhotoItemDTO
        {
            Id = photoItem.Id,
            AuthorId = photoItem.AuthorId,
            PublicationDate = photoItem.PublicationDate,
            Description = photoItem.Description,
            Hashtags = photoItem.Hashtags
        };

        return photoItemDTO;
    }
}