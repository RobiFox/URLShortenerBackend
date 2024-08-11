﻿using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost]
    public IActionResult Post(string url) {
        string id;
        do {
            id = Guid.NewGuid().ToString("N")[..8];
        } while (context.UrlPairs.Any(e => e.Id == id));
        IdUrlPair pair = new IdUrlPair {
            Url = url,
            Id = id
        };
        context.UrlPairs.Add(pair);
        context.SaveChanges();
        return Ok(pair);
    }
}