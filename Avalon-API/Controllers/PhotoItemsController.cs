using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;
using System.Linq.Expressions;
using System.Linq;

namespace Avalon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotoItemsController : ControllerBase
{
    private readonly PhotoContext _context;
    private readonly UserContext _userContext;
    private readonly ILogger _logger;


    public PhotoItemsController(PhotoContext context, UserContext userContext, ILogger<UsersController> logger)
    {
        _context = context;
        _userContext = userContext;
        _logger = logger;
    }

    // GET: api/PhotoItems
    [HttpGet]
    [TimerAspect]
    public async Task<ActionResult<IEnumerable<PhotoItemDTO>>> GetPhotoItems([FromQuery(Name = "filter")] string filter = "")
    {
        List<PhotoItem>? photoItems;
        if (filter != "")
        {
            var spec = new PhotoSpecification(filter);
            photoItems = await _context.PhotoItems.Where(spec.Criteria).ToListAsync();
        }
        else
        {
            photoItems = await _context.PhotoItems.ToListAsync();
        }
        
        // var photoItemsDTO = new List<PhotoItemDTO>();

        // foreach (var photoItem in photoItems)
        // {
        //     var dto = ItemToDTO(photoItem);

        //     if (!string.IsNullOrEmpty(photoItem.ImagePath))
        //     {
        //         var imagePath = Path.Combine("./images", photoItem.ImagePath);
        //         dto.ImageData = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(imagePath));
        //     }

        //     photoItemsDTO.Add(dto);
        // }


        var photoItemsDTO = photoItems.Select(async photoItem =>
        {
            var dto = ItemToDTO(photoItem);

            if (!string.IsNullOrEmpty(photoItem.ImagePath))
            {
                var imagePath = Path.Combine("./images", photoItem.ImagePath);
                dto.ImageData = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(imagePath));
            }

            return dto;
        }).ToList();

        _logger.LogInformation("GET photos at {DT} by user_id: 1",
            DateTime.UtcNow.ToLongTimeString());

        return Ok(photoItemsDTO);
    }

    // GET: api/PhotoItems/5
    // <snippet_GetByID>
    [TimerAspect]
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

        _logger.LogInformation("GET photo by id at {DT} by user_id: 1",
            DateTime.UtcNow.ToLongTimeString());

        return dto;
    }

    // PUT: api/PhotoItems/5
    [ErrorHandlingAspect]
    [TimerAspect]
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

        _logger.LogInformation("PUT photo id: '{ID}' at {DT} by author_id : '{AUTHOR}'",
            photoDTO.Id, DateTime.UtcNow.ToLongTimeString(), photoDTO.AuthorId);

        return NoContent();
    }

    // POST: api/PhotoItems
    [ErrorHandlingAspect]
    [TimerAspect]
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
            var imageBytes = Convert.FromBase64String(photoDTO.ImageData);
            var fileName = Guid.NewGuid().ToString() + ".jpg";
            //var filePath = Path.Combine("/app/images", fileName);
            var filePath = Path.Combine("./images", fileName);


            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
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

        _logger.LogInformation("POST photo id: '{ID}' at {DT} by author_id : '{AUTHOR}'",
            photoDTO.Id, DateTime.UtcNow.ToLongTimeString(), photoDTO.AuthorId);

        return CreatedAtAction(
            nameof(GetPhotoItem),
            new { id = photoItem.Id },
            ItemToDTO(photoItem));
    }

    // DELETE: api/PhotoItems/5
    [HttpDelete("{id}")]
    [TimerAspect]
    public async Task<IActionResult> DeletePhotoItem(long id)
    {
        var photoItem = await _context.PhotoItems.FindAsync(id);
        if (photoItem == null)
        {
            return NotFound();
        }

        _context.PhotoItems.Remove(photoItem);
        await _context.SaveChangesAsync();

        _logger.LogInformation("PUT photo at {DT} by author_id : '{AUTHOR}'",
            DateTime.UtcNow.ToLongTimeString(), id);

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