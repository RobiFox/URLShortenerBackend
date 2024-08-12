using Microsoft.EntityFrameworkCore;

namespace url_shorten_backend;

public class UrlDbContext : DbContext {
    public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }
    
    public DbSet<IdUrlPair> UrlPairs { get; set; }
}