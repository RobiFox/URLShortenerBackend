using Microsoft.AspNetCore.Mvc;

namespace url_shorten_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController(UrlDbContext context) : ControllerBase {
    [HttpGet("{id}")]
    public IActionResult Get(string id) {
        IdUrlPair? pair = context.UrlPairs.FirstOrDefault(e => e.Id == id);
        if (pair == null) {
            return NotFound($"No URL with {id}");
        }
        return Redirect(pair.Url);
    }
    
    [HttpGet]
    public IActionResult GetOwner(string? owner) {
        if (owner == null) {
            return Ok(context.UrlPairs.ToList());
        }
        return Ok(context.UrlPairs.Where(pair => pair.UploaderId == owner).Select(pair => new IdUrlPairDto(pair)).ToList());
    }
    
    [HttpPost]
    public IActionResult Post([FromQuery] string url, [FromQuery] string owner) {
        string id;
        do {
            id = Guid.NewGuid().ToString("N")[..8];
        } while (context.UrlPairs.Any(e => e.Id == id));
        IdUrlPair pair = new IdUrlPair {
            Url = url,
            Id = id,
            UploaderId = owner
        };
        context.UrlPairs.Add(pair);
        context.SaveChanges();
        return Ok(pair);
    }

    [HttpDelete]
    [ServiceFilter(typeof(UrlOwnershipRequiredAttribute))]
    public IActionResult Delete(string id, string owner) {
        IdUrlPair? pair = context.UrlPairs.FirstOrDefault(e => e.Id == id);
        if (pair == null) {
            return NotFound($"No URL with {id}");
        }
        context.UrlPairs.Remove(pair);
        context.SaveChanges();
        return Ok(owner);
    }
}