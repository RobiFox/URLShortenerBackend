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
        return Ok(pair);
    }
    
    [HttpGet]
    public IActionResult Get() {
        return Ok(context.UrlPairs.ToList());
    }
    
    [HttpGet]
    public IActionResult GetOwner(string owner) {
        return Ok(context.UrlPairs.Where(pair => pair.UploaderId == owner).Select(pair => new IdUrlPairDto(pair)).ToList());
    }
    
    [HttpPost]
    public IActionResult Post(string url, string owner) {
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
        if (HttpContext.Items["UrlPair"] is not IdUrlPair pair) {
            return NotFound("URL Pair not found");
        }
        context.UrlPairs.Remove(pair);
        context.SaveChanges();
        return Ok(owner);
    }
}